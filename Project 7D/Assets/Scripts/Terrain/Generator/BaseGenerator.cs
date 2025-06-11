using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class BaseGenerator : BiomeGenerator
{
    public override void GenerateBiome()
    {
        // 예시: 지형을 평지로 초기화
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        float[,] heights = new float[data.heightmapResolution, data.heightmapResolution];
        data.SetHeights(0, 0, heights);
    }
}
