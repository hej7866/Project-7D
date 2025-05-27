using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("채집 로딩 UI")]
    [SerializeField] private GameObject gatherUI;
    [SerializeField] private Image gatherBar;

    public void ShowGatherUI()
    {
        gatherUI.SetActive(true);
    }

    public void HideGatherUI()
    {
        gatherUI.SetActive(false);
        gatherBar.fillAmount = 0f;
    }

    public void SetGatherProgress(float ratio)
    {
        gatherBar.fillAmount = Mathf.Clamp01(ratio);
    }
}
