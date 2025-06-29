using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : SingleTon<ShopManager>
{
    [SerializeField] private bool inShopArea;

    [Header("아이템 샵")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject itemSlotPanel;
    public Text itemName;
    public Text itemDesc;
    public Text requirements_Resource;
    public Text requirements_Value;
    [SerializeField] private List<ItemData> ToweritemDatas;
    [SerializeField] private List<ItemData> FooditemDatas;

    

    void Update()
    {
        if (inShopArea)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shopUI.SetActive(true);
            }
        }
        else if (!inShopArea)
        {
            if (shopUI.activeSelf) shopUI.SetActive(false);
        }
    }

    void Start()
    {
        foreach (ItemData itemData in ToweritemDatas)
        {
            GameObject itemSlotInstance = Instantiate(itemSlotPrefab, itemSlotPanel.transform);
            SetItemData(itemSlotInstance, itemData);
        }
    }

    void SetItemData(GameObject itemSlotInstance, ItemData itemData)
    {
        ItemSlot itemSlot = itemSlotInstance.GetComponent<ItemSlot>();
        itemSlot.currentData = itemData;
    }

    void OnTriggerEnter(Collider other)
    {
        inShopArea = true;
    }

    void OnTriggerExit(Collider other)
    {
        inShopArea = false;
    }
}
