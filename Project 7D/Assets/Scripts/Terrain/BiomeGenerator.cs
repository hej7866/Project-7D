using UnityEngine;
using System.Collections.Generic;

public abstract class BiomeGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ResourceType
    {
        public string tag; // 풀 태그
        public int spawnCount;
        public BiomeType allowedBiome;
    }

    [Header("바이옴 중심 위치 (월드 좌표)")]
    public Vector3 CenterPosition;

    [Header("영역 크기")]
    public int BiomeSize = 512;

    [Header("자동 생성 여부")]
    public bool GenerateOnStart = true;

    [Header("바이옴")]
    public BiomeType biome;

    [Header("자원 배치 설정")]
    public List<ResourceType> resources;
    public float minSpacing = 4f;

    protected List<Vector3> placedPositions = new();
    protected Terrain terrain;

    protected virtual void Awake()
    {
        terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        data.size = new Vector3(BiomeSize, 100f, BiomeSize);

        // 중심 위치에 맞게 Terrain 배치 (선택)
        transform.position = new Vector3(
            CenterPosition.x - BiomeSize / 2f,
            CenterPosition.y,
            CenterPosition.z - BiomeSize / 2f
        );
    }

    protected virtual void Start()
    {
        if (GenerateOnStart)
        {
            GenerateBiome();
            GenerateResources();
        }
    }

    public abstract void GenerateBiome();


    /// <summary>
    /// 리소스 생산을 담당하는 로직
    /// </summary>
    protected virtual void GenerateResources()
    {
        foreach (var resource in resources)
        {
            SpawnResources(resource);
        }
    }

    protected void SpawnResources(ResourceType resourceType)
    {
        int placed = 0, attempts = 0;
        int maxAttempts = resourceType.spawnCount * 10;

        Vector3 min = transform.position;
        Vector3 max = transform.position + new Vector3(BiomeSize, 0, BiomeSize);

        while (placed < resourceType.spawnCount && attempts < maxAttempts)
        {
            float x = Random.Range(min.x, max.x);
            float z = Random.Range(min.z, max.z);
            Vector3 pos = new Vector3(x, 0, z);
            pos.y = terrain.SampleHeight(pos);

            if (!IsPositionValid(pos)) { attempts++; continue; }

            ObjectPool.Instance.SpawnFromPool(resourceType.tag, pos, Quaternion.identity);
            placedPositions.Add(pos);
            placed++;
        }
    }

    /// <summary>
    /// 여기 리소스를 배치할 수 있는가를 판단하는 메서드
    /// 최소 거리랑 비교하여 보다 가까우면 펄스를 반환 멀다면 트루를 반환
    /// </summary>
    /// <param name="pos">리소스를 배치할려는 위치</param>
    /// <returns></returns>
    protected bool IsPositionValid(Vector3 pos)
    {
        foreach (var p in placedPositions)
        {
            if (Vector3.Distance(pos, p) < minSpacing)
                return false;
        }
        return true;
    }
}
