using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class CityGenerator : BiomeGenerator
{
    [SerializeField] private GameObject[] townPrefabs;
    [SerializeField] private GameObject city;

    public override void GenerateBiome()
    {
        // 예시: 지형을 평지로 초기화
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        float[,] heights = new float[data.heightmapResolution, data.heightmapResolution];
        data.SetHeights(0, 0, heights);

        BuildTown();
    }

    void BuildTown()
    {
        for (int x = 356; x <= 656; x += 300)
        {
            for (int y = -156; y <= 156; y += 100)
            {
                Vector3 pos = new Vector3(x, 0, y);
                Instantiate(townPrefabs[0], pos, Quaternion.identity, city.transform);
            }
        }
        
    }
}
