using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingleTon<TimeManager>
{
    public enum TimeOfDay { Dawn, Day, Evening, Night }

    [Header("하루 설정")]
    [SerializeField] private float secondsPerDay = 600f;
    public TimeOfDay CurrentTimeOfDay { get; private set; }

    public int CurrentDay;
    [SerializeField] private float currentTime = 0f; // 인스펙터 획인용

    public float DayPercent => currentTime / secondsPerDay;

    public event Action<TimeOfDay> OnTimeOfDayChanged;
    public event Action<int> OnNewDay;


    public event Action StartWave;
    public event Action EndWave;

    [Header("낮 / 밤 여부")]
    public bool isNightTime = false;
    public bool isDayTime = true;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= secondsPerDay)
        {
            currentTime = 0f;
            CurrentDay++;
            OnNewDay.Invoke(CurrentDay);
        }

        UpdateTimePhase();

        OnNightTime();
        OnDayTime();
    }

    void UpdateTimePhase()
    {
        TimeOfDay newPhase;

        if (DayPercent < 0.2f) newPhase = TimeOfDay.Dawn;
        else if (DayPercent < 0.5f) newPhase = TimeOfDay.Day;
        else if (DayPercent < 0.8f) newPhase = TimeOfDay.Evening;
        else newPhase = TimeOfDay.Night;

        if (newPhase != CurrentTimeOfDay)
        {
            CurrentTimeOfDay = newPhase;
            OnTimeOfDayChanged?.Invoke(CurrentTimeOfDay);
        }
    }

    void OnNightTime()
    {
        if (isNightTime) return;

        if (currentTime / 25 >= 20) // 20시 이후
        {
            StartWave.Invoke();
            isNightTime = true;
            isDayTime = false;
        }
    }

    void OnDayTime()
    {
        if (isDayTime) return;

        if (currentTime / 25 >= 6 && currentTime / 25 <= 20) // 6시에서 20시 사이
        {
            EndWave.Invoke();
            isDayTime = true;
            isNightTime = false;
        }
    }
}
