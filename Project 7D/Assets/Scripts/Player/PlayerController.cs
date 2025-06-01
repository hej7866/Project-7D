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

    private int _hunger;
    private int _thirst;
    private float _stamina;

    // 읽기전용 값 할당은 _변수로 !!
    public int Hunger => _hunger; 
    public int Thirst => _thirst;
    public float Stamina => _stamina;

    [SerializeField] private float hungerDecreaseRate = 1f; // 초당 1
    [SerializeField] private float thirstDecreaseRate = 1.2f;
    [SerializeField] private float staminaDecreaseRate = 30f;
    [SerializeField] private float staminaRecoverRate = 20f;

    public event Action<float> OnPlayerHealthChanged;


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
    private Rigidbody rb;
    private Animator animator;

    private Vector3 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        PlayerHealth = maxHealth;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        moveDir = new Vector3(h, 0, v).normalized;

        if (h == 0 && v == 0)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);

        // 방향 회전
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, 360 * Time.fixedDeltaTime));
        }

        //UpdatePlayerCondition();

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdatePlayerCondition()
    {
        // 생존 스탯 감소
        _hunger = Mathf.Max(0, _hunger - Mathf.FloorToInt(hungerDecreaseRate * Time.deltaTime));
        _thirst = Mathf.Max(0, _thirst - Mathf.FloorToInt(thirstDecreaseRate * Time.deltaTime));

        // 스태미너 조절
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && moveDir != Vector3.zero;

        if (isRunning && _stamina > 0)
        {
            _stamina -= staminaDecreaseRate * Time.deltaTime;
            moveSpeed = 8f;
        }
        else
        {
            _stamina += staminaRecoverRate * Time.deltaTime;
            moveSpeed = 5f;
        }

        _stamina = Mathf.Clamp(_stamina, 0f, maxStamina);

        // 배고픔/목마름 페널티
        if (_hunger == 0 || _thirst == 0)
        {
            PlayerHealth -= 1; // 서서히 체력 감소
        }
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;

        PlayerHealth -= damage;
        Debug.Log($"{damage}를 입음 남은 체력 : {PlayerHealth}");
    }

    public void Eat(int amount) // 먹기
    {
        _hunger = Mathf.Clamp(_hunger + amount, 0, maxHunger);
    }

    public void Drink(int amount) // 마시기
    {
        _thirst = Mathf.Clamp(_thirst + amount, 0, maxThirst);
    }

    public void RestoreStamina(float amount) // 스테미너 회복
    {

        _stamina = Mathf.Clamp(_stamina + amount, 0f, maxStamina);
    }

    void Die()
    {
        Debug.Log("플레이어가 사망하였습니다.");
    }
}
