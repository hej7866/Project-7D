using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class SnowGenerator : BiomeGenerator
{
    [Header("펄린 노이즈 스케일")]
    [SerializeField] private float noiseScale = 0.005f;

    [Header("높이 배율")]
    [SerializeField] private float heightMultiplier = 0.4f;

    [Header("랜덤 시드")]
    [SerializeField] private int seed = 0;

    private float offsetX;
    private float offsetZ;

    public override void GenerateBiome()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution, resolution];

        // 시드 기반 랜덤 오프셋
        System.Random prng = new System.Random(seed == 0 ? Random.Range(1, int.MaxValue) : seed);
        offsetX = prng.Next(-100000, 100000);
        offsetZ = prng.Next(-100000, 100000);

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float worldX = (float)x / resolution * BiomeSize + CenterPosition.x + offsetX;
                float worldZ = (float)z / resolution * BiomeSize + CenterPosition.z + offsetZ;

                float noise = Mathf.PerlinNoise(worldX * noiseScale, worldZ * noiseScale);
                heights[z, x] = noise * heightMultiplier;
            }
        }

        data.SetHeights(0, 0, heights);
    }
}
