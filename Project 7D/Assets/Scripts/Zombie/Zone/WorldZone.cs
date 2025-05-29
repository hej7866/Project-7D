using UnityEngine;
using System.Collections.Generic;

public class WorldZone
{
    public Vector2Int zoneID;
    public Vector3 centerPos;
    public BiomeType biome;
    public float baseSpawnChance;
    public List<GameObject> zombiesInZone = new();
}
