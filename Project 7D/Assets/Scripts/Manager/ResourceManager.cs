using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    [System.Serializable]
    public class ResourceType
    {
        public string tag; // 풀 태그
        public int spawnCount;
        public BiomeType allowedBiome;
    }

    public List<ResourceType> resources;

    [Header("배치 범위")]
    public Vector3 terrainMin = new Vector3(0, 0, 0);
    public Vector3 terrainMax = new Vector3(2048, 0, 2048);
    public Vector3 baseCenter = new Vector3(1024, 0, 1024);
    public float baseSize = 64f;

    public float minSpacing = 4f;
    private List<Vector3> placedPositions = new();

    void Start()
    {
        foreach (var resource in resources)
        {
            SpawnResources(resource);
        }
    }

    void SpawnResources(ResourceType resourceType)
    {
        int placed = 0;
        int attempts = 0;

        while (placed < resourceType.spawnCount && attempts < resourceType.spawnCount * 100)
        {
            float x = Random.Range(terrainMin.x, terrainMax.x);
            float z = Random.Range(terrainMin.z, terrainMax.z);

            if (IsInsideBase(x, z)) { attempts++; continue; }

            Vector3 pos = new Vector3(x, 0f, z);
            pos.y = Terrain.activeTerrain.SampleHeight(pos);

            BiomeType biome = GetBiomeType(pos);

            if (biome != resourceType.allowedBiome || !IsPositionValid(pos))
            {
                attempts++;
                continue;
            }

            ObjectPool.Instance.SpawnFromPool(resourceType.tag, pos, Quaternion.identity);
            placedPositions.Add(pos);
            placed++;
            attempts++;
        }
    }

    bool IsInsideBase(float x, float z)
    {
        return Mathf.Abs(x - baseCenter.x) < baseSize / 2f &&
               Mathf.Abs(z - baseCenter.z) < baseSize / 2f;
    }

    bool IsPositionValid(Vector3 pos)
    {
        foreach (var p in placedPositions)
        {
            if (Vector3.Distance(pos, p) < minSpacing)
                return false;
        }
        return true;
    }

    BiomeType GetBiomeType(Vector3 pos)
    {
        const float center = 1024f;   // 월드 중앙
        const float baseHalf = 32f;   // 기지 반쪽 (64×64)

        float dx = pos.x - center;    // 중심에서의 오프셋
        float dz = pos.z - center;
        float absDx = Mathf.Abs(dx);
        float absDz = Mathf.Abs(dz);

        // 0. 중앙 기지 64 × 64
        if (absDx <= baseHalf && absDz <= baseHalf)
            return BiomeType.Base;

        // 1. 북쪽 설산 : z 쪽이 가장 크고, 북/남 대각선 위
        if (dz >= absDx)              // dz >= |dx|
            return BiomeType.Snow;

        // 2. 남쪽 사막
        if (dz <= -absDx)             // dz <= -|dx|
            return BiomeType.Desert;

        // 3. 동쪽 도시
        if (dx >= absDz)              // dx >= |dz|
            return BiomeType.City;

        // 4. 서쪽 숲/강
        if (dx <= -absDz)             // dx <= -|dz|
            return BiomeType.Forest;

        // 대각선 딱 선 위에 있을 때 – 취향대로
        return BiomeType.Base;
    }

}
