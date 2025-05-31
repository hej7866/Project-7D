using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

enum ZombieState { Idle, Chasing, Attacking }

public class SimpleZombieAI : MonoBehaviour
{
    ZombieState state = ZombieState.Idle;
    private Transform target;

    public ZombieStepAI zombieStepAI;


    [Header("좀비 설정")]
    public float zombieAttackRange = 0.3f;

    [Header("좀비 Animation")]
    [SerializeField] Animator anim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();

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
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttack", false);
        if (target != null && Vector3.Distance(transform.position, target.position) < 4f)
        {
            state = ZombieState.Chasing;
            zombieStepAI.currentStep = StepState.Step;
        }
    }

    void ChaseUpdate()
    {

        if (target == null) return;

        // 공격 범위 안으로 접근
        if (Vector3.Distance(transform.position, target.position) <= zombieAttackRange)
        {
            state = ZombieState.Attacking;
            zombieStepAI.currentStep = StepState.None;
            zombieStepAI.stepTimer = 0f;
            return;
        }

        anim.SetBool("isChasing", true);
        anim.SetBool("isAttack", false);
        ZombieMovement();

    }

    void AttackUpdate()
    {

        // 공격 범위 안으로 접근
        if (Vector3.Distance(transform.position, target.position) > zombieAttackRange)
        {
            state = ZombieState.Chasing;
            zombieStepAI.currentStep = StepState.Step;
            return;
        }

        anim.SetBool("isChasing", false);
        anim.SetBool("isAttack", true);
    }


    void ZombieMovement()
    {
        Vector3 dir = (target.localPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        //zombieStepAI.ZombieStep(target);
    }

}
