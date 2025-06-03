using UnityEngine;
using UnityEngine.UIElements;

public class SunManager : SingleTon<SunManager>
{
    public Light Sun;

    void Update()
    {
        if (Sun == null || TimeManager.Instance == null) return;

        float angle = Mathf.Lerp(-90f, 270f, TimeManager.Instance.DayPercent);
        Sun.transform.rotation = Quaternion.Euler(angle, 0, 0);
    }

}
