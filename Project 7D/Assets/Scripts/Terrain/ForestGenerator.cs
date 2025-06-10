using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public Terrain terrain;

    [Header("배치 설정")]
    public string treeTag = "Tree";

    private const int TerrainSize = 2048;
    private const int Center = TerrainSize / 2;
    private const int ForestStartX = 0;
    private const int ForestEndX = Center - 32; // BaseSize / 2 + buffer

    public void DecorateForest(int count, string tag)
    {
        for (int i = 0; i < count; i++)
        {
            // X는 서쪽 방향 사다리꼴
            float x = Random.Range(ForestStartX, ForestEndX);
            float xFromCenter = Mathf.Abs(x - Center);

            // z 범위는 사다리꼴의 높이에 비례해 제한
            float zHalfRange = Mathf.Lerp(0f, 512f, Mathf.InverseLerp(0f, Center, xFromCenter));
            float z = Random.Range(Center - zHalfRange, Center + zHalfRange);

            // Terrain 높이 얻기
            float y = terrain.SampleHeight(new Vector3(x, 0, z));

            // (생략: 앞서 설명한 서쪽 사다리꼴 범위 계산)
            Vector3 pos = new Vector3(x, y, z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            GameObject obj = ObjectPool.Instance.SpawnFromPool(tag, pos, rot);
            if (obj != null)
                obj.transform.SetParent(transform);
        }
    }

}
