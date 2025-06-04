using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Size")]
    public int Width = 64;
    public int Height = 64;
    public float CellSize = 1f;

    [Header("Tile Prefab")]
    public GameObject TilePrefab;

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
            }
        }
    }
}
