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

}
