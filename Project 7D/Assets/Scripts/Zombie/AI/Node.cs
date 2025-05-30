using UnityEngine;

public class Node
{
    public Vector2Int position;
    public bool isWalkable;
    public float gCost, hCost;
    public Node parent;

    public float fCost => gCost + hCost;

    public Node(Vector2Int pos, bool walkable)
    {
        position = pos;
        isWalkable = walkable;
    }
}
