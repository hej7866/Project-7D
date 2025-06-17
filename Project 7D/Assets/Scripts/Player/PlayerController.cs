using UnityEngine;
using UnityEngine.UIElements;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : SingleTon<PlayerController>
{
    [Header("플레이어 설정")]
    public float moveSpeed = 5f;

    [Header("생존 스탯")]
    public float maxHealth = 100f;
    public int maxHunger = 100;
    public int maxThirst = 100;
    public int maxStamina = 100;

    private int hunger;
    private int thirst;
    private float stamina;
    private float conditonTickInterval;

    // 읽기전용 값 할당은 _변수로 !!
    public int Hunger => hunger; 
    public int Thirst => thirst;
    public float Stamina => stamina;

    [SerializeField] private int hungerDecreaseRate = 1; // 초당 1
    [SerializeField] private int thirstDecreaseRate = 1;
    [SerializeField] private float staminaDecreaseRate = 1f;
    [SerializeField] private float staminaRecoverRate = 20f;

    public event Action<float> OnPlayerHealthChanged; // 체력 이벤트
    public event Action<float> OnPlayerStaminaChanged; // 체력 이벤트
    public event Action<int, int> OnPlayerConditonChanged;


    private float _playerHealth;
    public float PlayerHealth
    {
        get => _playerHealth;
        private set
        {
            _playerHealth = Mathf.Clamp(value, 0, maxHealth);
            OnPlayerHealthChanged.Invoke(_playerHealth);
            if (_playerHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    private bool isDead = false;
    private bool isRun = false;
    private Rigidbody rb;
    private Animator animator;

    private Vector3 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        conditonTickInterval = 0f;

        SetPlayerConditon();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        moveDir = new Vector3(h, 0, v).normalized;

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if (IsMove(h, v)) // 걷다가 -> 뛰기
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                isRun = true;
            }
            else // 멈추기
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                isRun = false;
            }
        }
        else
        {
            if (IsMove(h, v)) // 뛰다가 -> 걷기
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                isRun = false;
            }
            else // 멈추기
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                isRun = false;
            }
        }

        // 방향 회전
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, 360 * Time.fixedDeltaTime));
        }

        UpdatePlayerCondition();

    }

    void FixedUpdate()
    {
        if (!isRun) rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        else if(isRun) rb.MovePosition(rb.position + moveDir * moveSpeed * 2 * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 플레이어 컨디션 세팅
    /// </summary>
    void SetPlayerConditon()
    {
        PlayerHealth = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
        stamina = maxStamina;
    }

    bool IsMove(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            return false;
        }
        return true;
    }


    void UpdatePlayerCondition()
    {
        if (isRun)
        {
            stamina = Mathf.Max(0, stamina - staminaDecreaseRate * Time.deltaTime);
            OnPlayerStaminaChanged.Invoke(stamina);
        }
        else if (!isRun)
        {
            stamina = Mathf.Min(100, stamina + staminaRecoverRate * Time.deltaTime);
            OnPlayerStaminaChanged.Invoke(stamina);
        }

        conditonTickInterval += Time.deltaTime;
        if (conditonTickInterval >= 5f)
        {
            hunger -= hungerDecreaseRate;
            thirst -= thirstDecreaseRate;

            OnPlayerConditonChanged.Invoke(hunger, thirst);

            conditonTickInterval = 0f;
        }

        // 배고픔/목마름 페널티
        // if (hunger == 0 || thirst == 0)
        // {
        //     PlayerHealth -= 1; // 서서히 체력 감소
        // }
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;

        PlayerHealth -= damage;
        Debug.Log($"{damage}를 입음 남은 체력 : {PlayerHealth}");
    }

    public void Eat(int amount) // 먹기
    {
        hunger = Mathf.Clamp(hunger + amount, 0, maxHunger);
    }

    public void Drink(int amount) // 마시기
    {
        thirst = Mathf.Clamp(thirst + amount, 0, maxThirst);
    }

    public void RestoreStamina(float amount) // 스테미너 회복
    {

        stamina = Mathf.Clamp(stamina + amount, 0f, maxStamina);
    }

    void Die()
    {
        Debug.Log("플레이어가 사망하였습니다.");
    }
}
