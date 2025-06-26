using UnityEngine;
using UnityEngine.AI;

public class WaveZombieAI : MonoBehaviour
{
    public enum ZombieState { ToBase, ToPlayer, Attacking }
    [SerializeField] private ZombieState currentState = ZombieState.ToBase;

    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackPower = 5f;

    private Transform player;
    private Transform baseHeart;
    private NavMeshAgent agent;
    private Animator anim;

    private float detectInterval = 1f;
    private float detectTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseHeart = GameObject.FindGameObjectWithTag("Heart").transform;

        SetDestination(baseHeart.position);
    }

    void Update()
    {
        detectTimer += Time.deltaTime;
        if (detectTimer >= detectInterval)
        {
            detectTimer = 0f;
            UpdateTarget();
        }

        UpdateAnimation();

        if (currentState == ZombieState.Attacking)
        {
            FaceTarget(player);
            // 공격 조건 확인
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > attackRange)
            {
                currentState = ZombieState.ToPlayer;
                SetDestination(player.position);
            }
        }
    }

    void UpdateTarget()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float distToHeart = Vector3.Distance(transform.position, baseHeart.position);

        if (distToPlayer <= detectRange)
        {
            currentState = ZombieState.ToPlayer;
            SetDestination(player.position);

            if (distToPlayer <= attackRange)
            {
                currentState = ZombieState.Attacking;
                agent.isStopped = true;
            }
        }
        else
        {
            currentState = ZombieState.ToBase;
            SetDestination(baseHeart.position);
        }
    }

    void SetDestination(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    void UpdateAnimation()
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;
        anim.SetBool("isChasing", isMoving);
        anim.SetBool("isAttack", currentState == ZombieState.Attacking);
    }

    void FaceTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
    }

    // Animation Event로 연결
    public void ZombieAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerController.Instance.TakeDamage(attackPower);
        }
    }
}
