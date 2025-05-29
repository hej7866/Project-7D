#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ZoneManager : SingleTon<ZoneManager>
{
    [Header("설정")]
    public float zoneSize = 4f; // 한 청크의 크기
    public GameObject zombiePrefab;
    public LayerMask groundLayer;

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
        int x = Mathf.FloorToInt(pos.x / zoneSize);
        int z = Mathf.FloorToInt(pos.z / zoneSize);
        return new Vector2Int(x, z);
    }

    public void SetPlayer(Transform p)
    {
        player = p;
    }

    void CreateZone(Vector2Int id)
    {
        Vector3 centerPos = new Vector3(id.x * zoneSize + zoneSize / 2, 0, id.y * zoneSize + zoneSize / 2);
        WorldZone zone = new WorldZone
        {
            zoneID = id,
            centerPos = centerPos,
            biome = GetBiomeFromGround(centerPos),
            baseSpawnChance = 0.3f
        };
        zones.Add(id, zone);
    }

    void TrySpawnZombies(WorldZone zone)
    {
        float spawnChance = zone.baseSpawnChance * GetBiomeMultiplier(zone.biome) * GetDayMultiplier();
        if (Random.value < spawnChance)
        {
            int count = Random.Range(1, 4);
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = zone.centerPos + Random.insideUnitSphere * (zoneSize / 2);
                pos.y = 0;

                GameObject zombie = Instantiate(zombiePrefab, pos, Quaternion.identity);
                zone.zombiesInZone.Add(zombie);
            }
        }
    }

    float GetBiomeMultiplier(BiomeType biome) // 바이옴 보정계수
    {
        return biome switch
        {
            BiomeType.Plains => 0.5f,
            BiomeType.Desert => 1.5f,
            BiomeType.Snow => 1.8f,
            _ => 1f
        };
    }

    float GetDayMultiplier() // 날짜 보정계수
    {
        int day = DayNightManager.Instance.CurrentDay;
        return 1f + (day - 1) * 0.1f;
    }


    BiomeType GetBiomeFromGround(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * 1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            string tag = hit.collider.tag;

            return tag switch
            {
                "PlainTile" => BiomeType.Plains,
                "SnowTile" => BiomeType.Snow,
                "DesertTile" => BiomeType.Desert,
                _ => BiomeType.Plains // 기본값
            };
        }

        return BiomeType.Plains; // 땅 감지 안 됐을 때
    }
    



#if UNITY_EDITOR
    void OnDrawGizmos() // 기즈모 디버그용
    {
        if (zones == null) return;

        foreach (var zone in zones.Values)
        {
            Vector3 center = zone.centerPos;
            Vector3 size = new Vector3(zoneSize, 0.1f, zoneSize);

            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(center, size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);

            Handles.Label(center + Vector3.up * 0.5f, zone.zoneID.ToString());
        }
    }
#endif
}
