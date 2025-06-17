using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : SingleTon<UIManager>
{
    [Header("채집 로딩 UI")]
    [SerializeField] private GameObject gatherUI;
    [SerializeField] private Image gatherBar;

    [Header("인벤토리 UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;

    [Header("플레이어 컨디션 UI")]
    [SerializeField] private Image healthGatherBar;
    [SerializeField] private Image staminaGatherBar;
    [SerializeField] private Image hungerGatherBar;
    [SerializeField] private Image thirstGatherBar;

    [Header("시간 / 날짜 UI")]
    [SerializeField] private Text dayText;
    [SerializeField] private Text timeText;

    private Dictionary<ResourceType, InventorySlot> slotDict = new Dictionary<ResourceType, InventorySlot>();


    void Start()
    {
        PlayerInventory.Instance.OnResourceChanged += UpdateInventoryUI;
        PlayerController.Instance.OnPlayerHealthChanged += PlayerHealthGatherBarUI;
        PlayerController.Instance.OnPlayerStaminaChanged += PlayerStaminaGatherBarUI;
        PlayerController.Instance.OnPlayerConditonChanged += PlayerConditonGatherBarUI;
        TimeManager.Instance.OnNewDay += UpdateDayDisplay;
    }

    private bool isOpen = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
        }

        UpdateTimeDisplay();
    }

    public void UpdateTimeDisplay()
    {
        float dayProgress = TimeManager.Instance.DayPercent;
        float totalMinutes = 24f * 60f * dayProgress;

        int hour = Mathf.FloorToInt(totalMinutes / 60f);
        int minute = Mathf.FloorToInt(totalMinutes % 60f);

        timeText.text = $"{hour:00}:{minute:00}";
    }

    public void UpdateDayDisplay(int currentDay)
    {
        dayText.text = $"{currentDay} 일차";
    }

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

    public void UpdateInventoryUI(Sprite icon, ResourceType type, Category category, int amount)
    {
        if (slotDict.TryGetValue(type, out var slot)) // 만약 인벤에 이미 있는 아이템이라면
        {
            slot.SetData(icon, type.ToString(), category.ToString(), amount); // 수량만 갱신
        }
        else
        {
            GameObject slotPrefab = Instantiate(inventorySlotPrefab, inventoryPanel);
            InventorySlot newSlotPrefab = slotPrefab.GetComponent<InventorySlot>();
            newSlotPrefab.SetData(icon, type.ToString(), category.ToString(), amount);
            slotDict.Add(type, newSlotPrefab); // 새로 등록
        }
    }

    // 체력바 UI
    public void PlayerHealthGatherBarUI(float playerHealth)
    {
        healthGatherBar.fillAmount = playerHealth / PlayerController.Instance.maxHealth;
    }
    // 스테미너 UI
    public void PlayerStaminaGatherBarUI(float playerStamina)
    {
        staminaGatherBar.fillAmount = playerStamina / PlayerController.Instance.maxStamina;
    }
    // 배고픔, 목마름 UI
    public void PlayerConditonGatherBarUI(int hunger, int thirst)
    {
        hungerGatherBar.fillAmount = (float)hunger / PlayerController.Instance.maxHunger;
        thirstGatherBar.fillAmount = (float)thirst / PlayerController.Instance.maxThirst;
    }
}

