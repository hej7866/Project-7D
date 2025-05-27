using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Size")]
    public int width = 64;
    public int height = 64;
    public float cellSize = 1f;

    [Header("Tile Prefab")]
    public GameObject tilePrefab;

    [Header("Tree / Rock Prefabs")]
    public GameObject[] treePrefabs;
    public GameObject[] rockPrefabs;

    [Range(0f, 1f)] public float treeSpawnChance = 0.003f;
    [Range(0f, 1f)] public float rockSpawnChance = 0.005f;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 tilePos = new Vector3(x * cellSize, 0f, z * cellSize);
                Instantiate(tilePrefab, tilePos, Quaternion.identity, transform);

                float rand = Random.value;

                if (rand < treeSpawnChance)
                {
                    int index = Random.Range(0, treePrefabs.Length);
                    Instantiate(treePrefabs[index], tilePos, Quaternion.identity, transform);
                }
                else if (rand < treeSpawnChance + rockSpawnChance)
                {
                    Vector3 newTilePos = new Vector3(tilePos.x, tilePos.y + 0.2f, tilePos.z);
                    int index = Random.Range(0, rockPrefabs.Length);
                    if (index != 2)
                    {
                        Instantiate(rockPrefabs[index], newTilePos, Quaternion.identity, transform);
                    }
                    else if (index == 2)
                    { 
                        Instantiate(rockPrefabs[index], tilePos, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
