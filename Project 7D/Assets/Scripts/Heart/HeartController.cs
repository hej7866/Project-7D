using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : SingleTon<HeartController>
{
    [Header("심장 설정")]
    public float MaxHealth = 500f;

    //public event Action<float> OnHeartHealthChanged;

    public bool isDestroyed = false;

    private float _heartHealth;
    public float HeartHealth
    {
        get => _heartHealth;
        private set
        {
            _heartHealth = Mathf.Clamp(value, 0, MaxHealth);
            Debug.Log($"{_heartHealth}");
            if (_heartHealth <= 0 && !isDestroyed)
            {
                Destroyed();
            }
        }
    }

    void Start()
    {
        HeartHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        HeartHealth -= damage;
    }

    void Destroyed()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
