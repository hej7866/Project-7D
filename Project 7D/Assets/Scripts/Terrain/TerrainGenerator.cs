using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{
    private const int TerrainSize = 2048;       // 실제 Terrain 크기
    private const int FieldSize = 1088;         // 우리가 사용하는 영역
    private const int BaseSize = 64;            // 중앙 기지
    private const int Height = 100;             // 최대 높이
    private const int Center = TerrainSize / 2; // 전체 Terrain 중심 = (1024,1024)

    [Header("노이즈 스케일")]
    public float forestScale = 100f;
    public float desertScale = 150f;
    public float cityScale = 300f;
    public float snowScale = 80f;

    private float offsetX;
    private float offsetZ;

    void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = TerrainSize + 1;
        terrainData.size = new Vector3(TerrainSize, Height, TerrainSize);

        float[,] heights = new float[TerrainSize, TerrainSize];

        int halfField = FieldSize / 2;

        for (int x = 0; x < TerrainSize; x++)
        {
            for (int z = 0; z < TerrainSize; z++)
            {
                // 사용 범위 내부만 계산 (중앙 1088x1088)
                if (Mathf.Abs(x - Center) > halfField || Mathf.Abs(z - Center) > halfField)
                {
                    heights[z, x] = 0f; // 외곽은 평지 또는 나중에 절벽/장식
                    continue;
                }

                int dx = x - Center;
                int dz = z - Center;

                float height = 0f;

                // 중앙 기지 영역은 완전 평지
                if (Mathf.Abs(dx) <= BaseSize / 2 && Mathf.Abs(dz) <= BaseSize / 2)
                {
                    height = 0f;
                }
                else if (Mathf.Abs(dz) > Mathf.Abs(dx))
                {
                    // 남북
                    if (dz > 0) height = Mathf.PerlinNoise((x + offsetX) / snowScale, (z + offsetZ) / snowScale) * 0.2f; // 북쪽 = 설산
                    else        height = Mathf.PerlinNoise((x + offsetX) / desertScale, (z + offsetZ) / desertScale) * 0.2f; // 남쪽 = 사막
                }
                else
                {
                    // 동서
                    if (dx > 0) height = Mathf.PerlinNoise((x + offsetX) / cityScale, (z + offsetZ) / cityScale) * 0.2f; // 동쪽 = 도시
                    else        height = Mathf.PerlinNoise((x + offsetX) / forestScale, (z + offsetZ) / forestScale) * 0.2f; // 서쪽 = 숲
                }

                heights[z, x] = height;
            }
        }

        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }
}
