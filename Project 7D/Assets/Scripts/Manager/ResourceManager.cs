using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Map Size")]
    public int Width = 64;
    public int Height = 64;
    public float CellSize = 1f;


    [Header("Tree / Rock Prefabs")]
    public GameObject[] TreePrefabs;
    public GameObject[] RockPrefabs;

    [Range(0f, 1f)] public float TreeSpawnChance = 0.003f;
    [Range(0f, 1f)] public float RockSpawnChance = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateResource();
    }

    void GenerateResource()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                Vector3 resourcePos = new Vector3(x * CellSize, 0f, z * CellSize);
                float rand = Random.value;

                if (rand < TreeSpawnChance)
                {
                    int index = Random.Range(0, TreePrefabs.Length);
                    Instantiate(TreePrefabs[index], resourcePos, Quaternion.identity, transform);
                }
                else if (rand < TreeSpawnChance + RockSpawnChance)
                {
                    Vector3 newTilePos = new Vector3(resourcePos.x, resourcePos.y + 0.2f, resourcePos.z);
                    int index = Random.Range(0, RockPrefabs.Length);
                    if (index != 2)
                    {
                        Instantiate(RockPrefabs[index], newTilePos, Quaternion.identity, transform);
                    }
                    else if (index == 2)
                    { 
                        Instantiate(RockPrefabs[index], resourcePos, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
