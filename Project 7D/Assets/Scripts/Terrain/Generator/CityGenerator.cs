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
        for (int x = 356; x <= 656; x += 100)
        {
            for (int z = -156; z <= 156; z += 300)
            {
                Vector3 pos = new Vector3(x, 0, z);
                Instantiate(townPrefabs[0], pos, Quaternion.Euler(0, 90, 0), city.transform);
            }
        }
        
    }
}
