using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingleTon<TimeManager>
{
    public enum TimeState {Day, Night }

    [Header("하루 설정")]
    [SerializeField] private float secondsPerDay = 600f;
    public TimeState CurrentTimeState;

    public int CurrentDay;
    [SerializeField] private float currentTime = 0f; // 인스펙터 획인용

    public float TimeOfDay => currentTime / 75;
    public float DayPercent => currentTime / secondsPerDay;

    public event Action<int> OnNewDay;
    public event Action StartWave;
    public event Action EndWave;




    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= secondsPerDay)
        {
            currentTime = 0f;
            CurrentDay++;
            OnNewDay.Invoke(CurrentDay);
        }
        OnNightTime();
        OnDayTime();
    }



    void OnNightTime()
    {
        if (CurrentTimeState == TimeState.Night) return;

        if (TimeOfDay >= 20) // 20시 이후
        {
            StartWave.Invoke();
            CurrentTimeState = TimeState.Night;
        }
    }

    void OnDayTime()
    {
        if (CurrentTimeState == TimeState.Day) return;

        if (TimeOfDay >= 6 && TimeOfDay <= 20) // 6시에서 20시 사이
        {
            EndWave.Invoke();
            CurrentTimeState = TimeState.Day;
        }
    }
}
