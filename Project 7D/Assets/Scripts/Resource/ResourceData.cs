using UnityEngine;

public enum ResourceType
{
    Tree,
    Rock,
}

public enum Category
{
    food,
    resource,
}


[CreateAssetMenu(fileName = "New ResourceData", menuName = "Game/Resource Data")]
public class ResourceData : ScriptableObject
{
    public ResourceType resourceType;
    public string displayName;
    public float gatherTime = 3f; // 이거로만 수확 시간 제어
    public Sprite icon;
    public Category category;
    public int amount;
    public AudioClip hitSound;
}
