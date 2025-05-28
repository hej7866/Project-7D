using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("채집 로딩 UI")]
    [SerializeField] private GameObject gatherUI;
    [SerializeField] private Image gatherBar;

    [Header("인벤토리 UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;

    private Dictionary<ResourceType, InventorySlot> slotDict = new Dictionary<ResourceType, InventorySlot>();

    void Start()
    {
        PlayerInventory.Instance.OnResourceChanged += UpdateInventoryUI;
    }

    private bool isOpen = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
        }
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
}

