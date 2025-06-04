using UnityEngine;

public class BaseWallBuilder : MonoBehaviour
{
    public GameObject wallPrefab;
    public float wallHeight = 30f;
    public float wallThickness = 2f;
    public int baseSize = 64;

    private const int TerrainSize = 2048;
    private const int Center = TerrainSize / 2;

    void Start()
    {
        Vector3 center = new Vector3(Center, 0, Center);
        float half = baseSize / 2f;

        // 북쪽 (Z+)
        BuildSingleWall(center + new Vector3(0, 0, half), new Vector3(baseSize, wallHeight, wallThickness));
        // 남쪽 (Z-)
        BuildSingleWall(center - new Vector3(0, 0, half), new Vector3(baseSize, wallHeight, wallThickness));
        // 동쪽 (X+)
        BuildSingleWall(center + new Vector3(half, 0, 0), new Vector3(wallThickness, wallHeight, baseSize));
        // 서쪽 (X-)
        BuildSingleWall(center - new Vector3(half, 0, 0), new Vector3(wallThickness, wallHeight, baseSize));
    }

    void BuildSingleWall(Vector3 pos, Vector3 scale)
    {
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + scale.y / 2f;

        GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
        wall.transform.localScale = scale;
        wall.tag = "BaseWall";
    }
}
