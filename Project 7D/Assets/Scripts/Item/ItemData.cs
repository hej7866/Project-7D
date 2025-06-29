using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum ItemType
{
    Tower,
    Food
}




[CreateAssetMenu(fileName = "New ItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public string desc;
    public Sprite icon;
    public ItemType itemType;
    public List<ResourceType> requirements_Resource;
    public int requirements_Value;
}
