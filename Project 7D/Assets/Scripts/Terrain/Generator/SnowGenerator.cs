using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class SnowGenerator : BiomeGenerator
{
    [Header("펄린 노이즈 스케일")]
    public float noiseScale = 0.005f;

    [Header("높이 배율")]
    public float heightMultiplier = 0.4f;

    public override void GenerateBiome()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution, resolution];

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float worldX = (float)x / resolution * BiomeSize + CenterPosition.x;
                float worldZ = (float)z / resolution * BiomeSize + CenterPosition.z;

                float noise = Mathf.PerlinNoise(worldX * noiseScale, worldZ * noiseScale);
                heights[z, x] = noise * heightMultiplier;
            }
        }

        data.SetHeights(0, 0, heights);
    }
}
