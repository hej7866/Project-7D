using UnityEngine;
using System.Collections.Generic;

public class WaveZombieAI : MonoBehaviour
{
    public enum ZombieState { ToBase, ToPlayer, Attacking }
    private ZombieState currentZombieState = ZombieState.ToBase;

    [SerializeField] private float detectPlayerRange = 10f;
    [SerializeField] private float zombieAttackRange = 0.3f;
    [SerializeField] private float zombieAttackPower = 5f;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector3 baseTarget;

    private Animator anim;
    private float detectInterval = 1f;
    private float detectTimer = 0f;

    private List<Vector3> path = new();
    private int pathIndex = 0;

    private GridManager grid;
    private AStarPathfinder pathfinder;

    void Start()
    {
        baseTarget = Vector3.zero;
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        grid = GridManager.Instance;
        pathfinder = new AStarPathfinder(grid);

        anim = GetComponent<Animator>();

        RequestPathTo(baseTarget);
    }

    void Update()
    {
        detectTimer += Time.deltaTime;
        if (detectTimer >= detectInterval)
        {
            detectTimer = 0f;
            CheckForPlayer();
        }

        if (currentZombieState == ZombieState.Attacking)
        {
            AttackUpdate();
            return;
        }

        FollowPath();
    }

    void CheckForPlayer()
    {
        if (playerTarget == null) return;

        float dist = Vector3.Distance(transform.position, playerTarget.position);

        if (dist <= detectPlayerRange)
        {
            if (currentZombieState != ZombieState.ToPlayer)
            {
                currentZombieState = ZombieState.ToPlayer;
                RequestPathTo(playerTarget.position);
            }
        }
        else
        {
            if (currentZombieState != ZombieState.ToBase)
            {
                currentZombieState = ZombieState.ToBase;
                RequestPathTo(baseTarget);
            }
        }

        if (dist <= zombieAttackRange)
        {
            currentZombieState = ZombieState.Attacking;
        }
    }

    void RequestPathTo(Vector3 worldTarget)
    {
        Vector2Int start = grid.WorldToGrid(transform.position);
        Vector2Int end = grid.WorldToGrid(worldTarget);

        Debug.Log($"[A* 요청] From {start} → {end}");

        Node startNode = grid.GetNode(start);
        Node endNode = grid.GetNode(end);

        if (startNode == null)
        {
            Debug.LogError("시작 노드가 null입니다!");
            return;
        }
        if (endNode == null)
        {
            Debug.LogError("도착 노드가 null입니다!");
            return;
        }
        if (!endNode.isWalkable)
        {
            Debug.LogError($"도착 노드가 막혀 있음! {endNode.position}");
            return;
        }

        List<Node> nodePath = pathfinder.FindPath(start, end);

        path.Clear();
        pathIndex = 0;

        if (nodePath == null || nodePath.Count == 0)
        {
            Debug.LogWarning($"경로 탐색 실패! (start={start}, end={end})");
            return;
        }

        Debug.Log($"경로 성공: {nodePath.Count} 노드");
        foreach (var node in nodePath)
            path.Add(grid.GridToWorld(node.position));
    }

    void FollowPath()
    {
        if (path.Count == 0 || pathIndex >= path.Count)
        {
            Debug.LogWarning($"{name} - 경로 없음 또는 도착함. path.Count = {path.Count}, pathIndex = {pathIndex}");
            return;
        }

        Vector3 targetPos = path[pathIndex];
        Vector3 dir = (targetPos - transform.position).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            pathIndex++;
        }

        anim.SetBool("isChasing", true);
        anim.SetBool("isAttack", false);
    }

    void AttackUpdate()
    {
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttack", true);

        if (Vector3.Distance(transform.position, playerTarget.position) > zombieAttackRange)
        {
            currentZombieState = ZombieState.ToPlayer;
            RequestPathTo(playerTarget.position);
        }
    }

    public void ZombieAttack()
    {
        if (Vector3.Distance(transform.position, playerTarget.position) > zombieAttackRange) return;

        PlayerController.Instance.TakeDamage(zombieAttackPower);
    }
}