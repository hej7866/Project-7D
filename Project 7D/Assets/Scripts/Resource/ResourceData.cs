using UnityEngine;

public enum ResourceType
{
    Tree,
    Rock,
}


[CreateAssetMenu(fileName = "New ResourceData", menuName = "Game/Resource Data")]
public class ResourceData : ScriptableObject
{
    public ResourceType resourceType;
    public string displayName;
    public float gatherTime = 3f; // 이거로만 수확 시간 제어
    public Sprite icon;
    public Color color;
    public AudioClip hitSound;
}
