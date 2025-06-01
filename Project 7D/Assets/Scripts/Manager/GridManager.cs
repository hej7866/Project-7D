using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int Width = 1024;
    public int Height = 1024;
    public float CellSize = 1/16f;
    public LayerMask ObstacleMask;

    private Node[,] grid;

    public void Awake()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        grid = new Node[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 worldPos = new Vector3(x * CellSize, 0, y * CellSize);
                bool walkable = !Physics.CheckSphere(worldPos, 0.4f, ObstacleMask);
                grid[x, y] = new Node(new Vector2Int(x, y), walkable);
            }
        }
    }

    public Node GetNode(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height)
            return grid[pos.x, pos.y];
        else
            return null;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / CellSize);
        int y = Mathf.RoundToInt(worldPos.z / CellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * CellSize, 0, gridPos.y * CellSize);
    }

    public List<Node> GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();
        Vector2Int[] directions = {
            new Vector2Int(0,1), new Vector2Int(1,0),
            new Vector2Int(0,-1), new Vector2Int(-1,0),
            new Vector2Int(1,1), new Vector2Int(1,-1),
            new Vector2Int(-1,1), new Vector2Int(-1,-1)
        };

        foreach (var dir in directions)
        {
            Vector2Int newPos = node.position + dir;
            Node neighbor = GetNode(newPos);

            if (neighbor != null && neighbor.isWalkable)
            {
                // 대각선 통과 제한 로직 (코너 끼우기 방지)
                if (Mathf.Abs(dir.x) == 1 && Mathf.Abs(dir.y) == 1)
                {
                    Node n1 = GetNode(node.position + new Vector2Int(dir.x, 0));
                    Node n2 = GetNode(node.position + new Vector2Int(0, dir.y));
                    if (n1 == null || n2 == null || !n1.isWalkable || !n2.isWalkable)
                        continue;
                }

                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
