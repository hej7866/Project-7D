using UnityEngine;
using UnityEngine.AI;

public class WaveZombieAI : MonoBehaviour
{
    public enum ZombieState { ToBase, ToPlayer, ToHeart, AttackPlayer, AttackHeart, }
    [SerializeField] private ZombieState currentState = ZombieState.ToBase;

    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackPlayerRange = 0.5f;
    [SerializeField] private float attackHeartRange = 2f;
    [SerializeField] private float attackPower = 5f;

    private Transform player;
    private Transform baseHeart;
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Transform target;

    private float detectInterval = 1f;
    private float detectTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseHeart = GameObject.FindGameObjectWithTag("Heart").transform;

        agent.avoidancePriority = Random.Range(40, 60);

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

        switch (currentState)
        {
            case ZombieState.ToPlayer:
                FaceTarget(target);
                break;
            case ZombieState.ToHeart:
                FaceTarget(target);
                break;
            case ZombieState.AttackPlayer:
                FaceTarget(target);

                float dist = Vector3.Distance(transform.position, player.position);
                if (dist > attackPlayerRange)
                {
                    currentState = ZombieState.ToPlayer;
                    SetDestination(target.position);
                }
                break;
            case ZombieState.AttackHeart:
                FaceTarget(target);
                break;
        }
    }

    void UpdateTarget()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float distToHeart = Vector3.Distance(transform.position, baseHeart.position);
        bool inBase = WaveManager.Instance.InBase(transform.position);

        if(inBase) BaseAI(distToPlayer, distToHeart);
    }

    void BaseAI(float distToPlayer, float distToHeart)
    {
        if (distToPlayer <= detectRange && distToHeart <= detectRange) // 둘다 거리안에있다면
        {
            currentState = ZombieState.ToPlayer;
            target = player;
            SetDestination(target.position);

            if (distToPlayer <= attackPlayerRange) // 추적하다가 공격사거리안에 들어오면
            {
                currentState = ZombieState.AttackPlayer;
                agent.isStopped = true;
            }
        }
        else if (distToPlayer <= detectRange && distToHeart > detectRange) // 플레이어만 거리안에있다면
        {
            currentState = ZombieState.ToPlayer;
            target = player;
            SetDestination(target.position);

            if (distToPlayer <= attackPlayerRange) // 추적하다가 공격사거리안에 들어오면
            {
                currentState = ZombieState.AttackPlayer;
                agent.isStopped = true;
            }
        }
        else
        {
            currentState = ZombieState.ToHeart;
            target = baseHeart;
            SetDestination(target.position);

            if (distToHeart <= attackHeartRange) // 추적하다가 공격사거리안에 들어오면
            {
                currentState = ZombieState.AttackHeart;
                agent.isStopped = true;
            }
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
        anim.SetBool("isRunning", isMoving);
        anim.SetBool("isAttack", currentState == ZombieState.AttackPlayer || currentState == ZombieState.AttackHeart);
    }

    void FaceTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            transform.rotation  = Quaternion.LookRotation(dir);
        }
    }

    // Animation Event로 연결
    public void ZombieAttack()
    {
        switch (currentState)
        {
            case ZombieState.AttackPlayer:
                if (Vector3.Distance(transform.position, player.position) <= attackPlayerRange)
                {
                    PlayerController.Instance.TakeDamage(attackPower);
                }
                break;
            case ZombieState.AttackHeart:
                if (Vector3.Distance(transform.position, baseHeart.position) <= attackHeartRange)
                {
                    HeartController.Instance.TakeDamage(attackPower);
                }
                break;
        }
    }
}
