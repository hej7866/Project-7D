using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Size")]
    public int Width = 64;
    public int Height = 64;
    public float CellSize = 1f;

    [Header("Tile Prefab")]
    public GameObject TilePrefab;

    [Header("Tree / Rock Prefabs")]
    public GameObject[] TreePrefabs;
    public GameObject[] RockPrefabs;

    [Range(0f, 1f)] public float TreeSpawnChance = 0.003f;
    [Range(0f, 1f)] public float RockSpawnChance = 0.005f;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                Vector3 tilePos = new Vector3(x * CellSize, 0f, z * CellSize);
                Instantiate(TilePrefab, tilePos, Quaternion.identity, transform);

                float rand = Random.value;

                if (rand < TreeSpawnChance)
                {
                    int index = Random.Range(0, TreePrefabs.Length);
                    Instantiate(TreePrefabs[index], tilePos, Quaternion.identity, transform);
                }
                else if (rand < TreeSpawnChance + RockSpawnChance)
                {
                    Vector3 newTilePos = new Vector3(tilePos.x, tilePos.y + 0.2f, tilePos.z);
                    int index = Random.Range(0, RockPrefabs.Length);
                    if (index != 2)
                    {
                        Instantiate(RockPrefabs[index], newTilePos, Quaternion.identity, transform);
                    }
                    else if (index == 2)
                    { 
                        Instantiate(RockPrefabs[index], tilePos, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
