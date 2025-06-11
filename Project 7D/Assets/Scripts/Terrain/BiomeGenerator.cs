using UnityEngine;

public abstract class BiomeGenerator : MonoBehaviour
{
    [Header("바이옴 중심 위치 (월드 좌표)")]
    public Vector3 CenterPosition;

    [Header("영역 크기")]
    public int BiomeSize = 512;

    [Header("자동 생성 여부")]
    public bool GenerateOnStart = true;

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
        }
    }

    public abstract void GenerateBiome();
}
