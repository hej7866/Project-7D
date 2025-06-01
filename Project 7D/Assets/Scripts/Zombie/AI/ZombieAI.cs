using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    enum ZombieState { Idle, Chasing, Attacking }
    ZombieState state = ZombieState.Idle;

    [Header("이동 설정")]
    public float speed = 2f;
    public float turnSpeed = 5f;
    public float repathInterval = 1.5f;
    public float attackRange = 1.5f;

    [Header("모델 회전")]
    public Transform visualModel; // 실제 회전시킬 모델 (없으면 자기 자신 회전)

    private GridManager grid;
    private AStarPathfinder pathfinder;

    private List<Vector3> smoothPath;     // 현재 사용 중인 경로
    private List<Vector3> pendingPath;    // 새로 계산된 경로 (대기 상태)
    private int _smoothIndex = 0;

    private Transform target;
    private float _lastPathTime = 0f;

    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        pathfinder = new AStarPathfinder(grid);
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (target != null)
        {
            state = ZombieState.Chasing;
            RequestNewPath();
        }
    }

    void Update()
    {
        switch (state)
        {
            case ZombieState.Idle:
                IdleUpdate();
                break;

            case ZombieState.Chasing:
                ChaseUpdate();
                break;

            case ZombieState.Attacking:
                AttackUpdate();
                break;
        }
    }

    void IdleUpdate()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < 4f)
        {
            state = ZombieState.Chasing;
            RequestNewPath();
        }
    }

    void ChaseUpdate()
    {
        if (target == null) return;

        // 공격 범위 안으로 접근
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            state = ZombieState.Attacking;
            return;
        }

        // 1초마다 경로 갱신
        if (Time.time - _lastPathTime > repathInterval)
        {
            RequestNewPath();
        }

        // 새 경로가 준비되었으면 부드럽게 교체
        if (pendingPath != null && pendingPath.Count >= 2)
        {
            smoothPath = pendingPath;
            _smoothIndex = GetClosestIndexOnPath(smoothPath, transform.position);
            pendingPath = null;
        }

        // 경로 없으면 추적 불가
        if (smoothPath == null || _smoothIndex >= smoothPath.Count)
            return;

        // 이동 처리
        Vector3 targetPos = smoothPath[_smoothIndex];
        Vector3 dir = (targetPos - transform.position);
        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);

        if (flatDir.magnitude > 0.05f)
        {
            Vector3 moveDir = flatDir.normalized;
            transform.position += moveDir * speed * Time.deltaTime;

            // 회전 처리
            Quaternion lookRot = Quaternion.LookRotation(moveDir);
            Transform rotateTarget = visualModel != null ? visualModel : transform;
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, lookRot, turnSpeed * Time.deltaTime);
        }

        // 다음 포인트로
        if (Vector3.Distance(transform.position, targetPos) < 0.2f)
            _smoothIndex++;
    }

    void AttackUpdate()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > attackRange)
        {
            state = ZombieState.Chasing;
            RequestNewPath();
            return;
        }

        Debug.Log("좀비 공격 중!");
    }

    void RequestNewPath()
    {
        Vector2Int start = grid.WorldToGrid(transform.position);
        Vector2Int end = grid.WorldToGrid(target.position);
        var nodePath = pathfinder.FindPath(start, end);
        if (nodePath == null || nodePath.Count < 2) return;

        List<Vector3> worldPath = new();
        foreach (var node in nodePath)
            worldPath.Add(grid.GridToWorld(node.position));

        pendingPath = CatmullRomUtility.GetSmoothPath(worldPath, 10);
        _lastPathTime = Time.time;
    }

    // 현재 위치와 가장 가까운 경로 인덱스 찾기
    int GetClosestIndexOnPath(List<Vector3> path, Vector3 currentPos)
    {
        int closest = 0;
        float minDist = float.MaxValue;

        for (int i = 0; i < path.Count; i++)
        {
            float dist = Vector3.Distance(currentPos, path[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }
        return closest;
    }

}
