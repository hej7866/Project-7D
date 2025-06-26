using UnityEngine;
using System.Collections.Generic;

public class GridManager : SingleTon<GridManager>
{
    public int Width = 512;
    public int Height = 512;
    public float CellSize = 1f;
    public LayerMask ObstacleMask;

    // 월드 좌표계 기준 그리드 시작점 (좌하단)
    public Vector2Int GridOrigin = new Vector2Int(-256, -256);
    [SerializeField] private float radius;

    private Node[,] grid;

    protected override void Awake()
    {
        base.Awake();
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        grid = new Node[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = GridToWorld(gridPos);
                
                bool walkable = !Physics.CheckSphere(worldPos, radius, ObstacleMask);

                grid[x, y] = new Node(gridPos, walkable);
            }
        }
    }

    /// <summary>
    /// 월드 좌표 → 그리드 인덱스 변환 (Grid 배열 접근용)
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - GridOrigin.x) / CellSize);
        int y = Mathf.FloorToInt((worldPos.z - GridOrigin.y) / CellSize);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// 그리드 인덱스 → 월드 좌표 (중심 기준)
    /// </summary>
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float worldX = GridOrigin.x + gridPos.x * CellSize + CellSize / 2f;
        float worldZ = GridOrigin.y + gridPos.y * CellSize + CellSize / 2f;
        return new Vector3(worldX, 0f, worldZ);
    }


    /// <summary>
    /// 월드 좌표 기준의 노드를 반환
    /// </summary>
    public Node GetNode(Vector2Int gridIndex)
    {
        if (gridIndex.x >= 0 && gridIndex.x < Width && gridIndex.y >= 0 && gridIndex.y < Height)
            return grid[gridIndex.x, gridIndex.y];
        return null;
    }

    /// <summary>
    /// 인접한 8방향의 노드 반환
    /// </summary>
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
            Vector2Int neighborWorldPos = node.position + dir;
            Node neighbor = GetNode(neighborWorldPos);

            if (neighbor != null && neighbor.isWalkable)
            {
                // 대각선 이동 시 코너 충돌 방지
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
