using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveZombieAI : MonoBehaviour
{
    public enum ZombieState { ToBase, ToPlayer }
    private ZombieState currentZombieState = ZombieState.ToBase;

    [Header("웨이브 좀비 설정")]
    [SerializeField] private float detectPlayerRange = 10f;
    [SerializeField] private float tempSpeed = 5f;
    [SerializeField] private float realSpeed = 1f;

    private float detectInterval = 1f;
    private float detectTimer = 0f;


    [Header("디버그용 타겟 세팅")]
    [SerializeField] private Vector3 baseTarget;
    [SerializeField] private Transform playerTarget;

    private Animator anim;

    void Start()
    {
        baseTarget = new Vector3(0, 0, 0);
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        detectTimer += Time.deltaTime;

        if (detectTimer >= detectInterval)
        {
            DetectingPlayer();
            detectTimer = 0f;
        }

        switch (currentZombieState)
        {
            case ZombieState.ToBase:
                Test(baseTarget);
                break;
            case ZombieState.ToPlayer:
                Test(playerTarget.position);
                break;
        }
    }

    void Test(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        anim.SetBool("isChasing", true);

        Vector2Int pos2D = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

        if (WaveManager.Instance.InBase(pos2D))
        {
            transform.position += dir * realSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += dir * tempSpeed * Time.deltaTime;
        }
    }

    void DetectingPlayer()
    {
        if (playerTarget == null) return;

        float distToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distToPlayer <= detectPlayerRange)
        {
            if (currentZombieState != ZombieState.ToPlayer)
            {
                currentZombieState = ZombieState.ToPlayer;
            }
        }
        else
        {
            if (currentZombieState != ZombieState.ToBase)
            {
                currentZombieState = ZombieState.ToBase;
            }
        }
    }
    
    
}
