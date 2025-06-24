#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ZoneManager : SingleTon<ZoneManager>
{
    [Header("설정")]
    public float ZoneSize = 4f; // 한 청크의 크기
    public GameObject ZombiePrefab;
    public LayerMask GroundLayer;
    public Terrain[] allTerrain;

    private Dictionary<Vector2Int, WorldZone> zones = new();
    private Transform player;



    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else return;
        }

        Vector2Int currentZone = GetPlayerZone();

        if (!zones.ContainsKey(currentZone))
        {
            CreateZone(currentZone);
            TrySpawnZombies(zones[currentZone]);
        }
    }


    Vector2Int GetPlayerZone()
    {
        Vector3 pos = player.position;
        int x = Mathf.FloorToInt(pos.x / ZoneSize);
        int z = Mathf.FloorToInt(pos.z / ZoneSize);
        return new Vector2Int(x, z);
    }

    public void SetPlayer(Transform p)
    {
        player = p;
    }

    void CreateZone(Vector2Int id)
    {
        Vector3 centerPos = new Vector3((id.x + 0.5f) * ZoneSize, 0f, (id.y + 0.5f) * ZoneSize);

        WorldZone zone = new WorldZone
        {
            zoneID = id,
            centerPos = centerPos,
            biome = GetBiome(centerPos),
            baseSpawnChance = 1f // TEST
        };
        zones.Add(id, zone);
    }

    void TrySpawnZombies(WorldZone zone)
    {
        Debug.Log($"{zone.biome}");
        float spawnChance = zone.baseSpawnChance * GetBiomeMultiplier(zone.biome) * GetDayMultiplier();
        if (Random.value < spawnChance)
        {
            int count = Random.Range(1, 4);
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = zone.centerPos + Random.insideUnitSphere * (ZoneSize / 2);
                pos = GetTerrainHeight(zone, pos);
                GameObject zombie = Instantiate(ZombiePrefab, pos, Quaternion.identity);
                zone.zombiesInZone.Add(zombie);
            }
        }
    }

    float GetBiomeMultiplier(BiomeType biome) // 바이옴 보정계수
    {
        return biome switch
        {
            BiomeType.Base => 0f,
            BiomeType.City => 0.5f,
            BiomeType.Forest => 0.8f,
            BiomeType.Desert => 1.5f,
            BiomeType.Snow => 1.8f,
            _ => 0f
        };
    }

    float GetDayMultiplier() // 날짜 보정계수
    {
        int day = TimeManager.Instance.CurrentDay;
        return 1f + (day - 1) * 0.1f;
    }

    Vector3 GetTerrainHeight(WorldZone zone, Vector3 pos)
    {
        switch (zone.biome)
        {
            case BiomeType.City:
                pos.y = allTerrain[0].SampleHeight(pos);
                break;
            case BiomeType.Forest:
                pos.y = allTerrain[1].SampleHeight(pos);
                break;
            case BiomeType.Desert:
                pos.y = allTerrain[2].SampleHeight(pos);
                break;
            case BiomeType.Snow:
                pos.y = allTerrain[3].SampleHeight(pos);
                break;
            case BiomeType.Base:
                pos.y = 0;
                break;
        }
        return pos;
    }


    BiomeType GetBiome(Vector3 worldPos)
    {
        Ray ray = new Ray(worldPos + Vector3.up * 50f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            int layer = hit.collider.gameObject.layer;

            if (layer == LayerMask.NameToLayer("City")) return BiomeType.City;
            else if (layer == LayerMask.NameToLayer("Forest")) return BiomeType.Forest;
            else if (layer == LayerMask.NameToLayer("Desert")) return BiomeType.Desert;
            else if (layer == LayerMask.NameToLayer("Snow")) return BiomeType.Snow;
        }
        return BiomeType.Base;
    }



#if UNITY_EDITOR
    void OnDrawGizmos() // 기즈모 디버그용
    {
        if (zones == null) return;

        foreach (var zone in zones.Values)
        {
            Vector3 center = zone.centerPos;
            Vector3 size = new Vector3(ZoneSize, 100f, ZoneSize);

            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(center, size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);

            Handles.Label(center + Vector3.up * 0.5f, zone.zoneID.ToString());
        }
    }
#endif
}
