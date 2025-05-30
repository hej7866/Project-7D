using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder
{
    private GridManager grid;

    public AStarPathfinder(GridManager gridManager)
    {
        grid = gridManager;
    }

    public List<Node> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        Node startNode = grid.GetNode(startPos);
        Node endNode = grid.GetNode(endPos);

        if (startNode == null || endNode == null || !endNode.isWalkable)
            return null;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        // 초기 비용 설정
        startNode.gCost = 0;
        startNode.hCost = Heuristic(startPos, endPos);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node current = openSet.OrderBy(n => n.fCost).ThenBy(n => n.hCost).First();

            if (current == endNode)
                return ReconstructPath(current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neighbor in grid.GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeG = current.gCost + Distance(current, neighbor);

                if (tentativeG < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor.position, endNode.position);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // 경로 없음
    }

    List<Node> ReconstructPath(Node endNode)
    {
        List<Node> path = new();
        Node current = endNode;
        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    float Heuristic(Vector2Int a, Vector2Int b)
    {
        // 맨해튼 거리
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy); // 대각선 가중치 반영
    }

    float Distance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.position.x - b.position.x);
        int dy = Mathf.Abs(a.position.y - b.position.y);
        return (dx == 1 && dy == 1) ? 14 : 10; // 대각선 14, 직선 10
    }
}
