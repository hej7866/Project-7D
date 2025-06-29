using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData currentData;

    void Start()
    {
        Image icon = GetComponent<Image>();
        icon.sprite = currentData.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopManager.Instance.itemName.text = currentData.displayName;
        ShopManager.Instance.itemDesc.text = currentData.desc;

        string requirements_Resource_text = string.Join(", ", currentData.requirements_Resource);
        ShopManager.Instance.requirements_Resource.text = $"필요한 재료 : {requirements_Resource_text}";
        ShopManager.Instance.requirements_Value.text = $"필요한 개수 : {currentData.requirements_Value}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShopManager.Instance.itemName.text = "아이템 이름";
        ShopManager.Instance.itemDesc.text = "아이템 설명입니다.";
        ShopManager.Instance.requirements_Resource.text = $"필요한 재료 : ";
        ShopManager.Instance.requirements_Value.text = $"필요한 개수 : ";
    }
}
