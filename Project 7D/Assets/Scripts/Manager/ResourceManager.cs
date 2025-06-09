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

        while (placed < resourceType.spawnCount && attempts < resourceType.spawnCount * 50)
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
               Mathf.Abs(z - baseCenter.y) < baseSize / 2f;
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
        if (pos.z > 1056f)
            return BiomeType.Snow;    // 북쪽 설산
        if (pos.z < 992f)
            return BiomeType.Desert;  // 남쪽 사막
        if (pos.x > 1056f)
            return BiomeType.City;    // 동쪽 도시
        if (pos.x < 992f)
            return BiomeType.Forest;  // 서쪽 숲/강

        return BiomeType.Base;      // 중앙 (기지 포함)
    }

}
