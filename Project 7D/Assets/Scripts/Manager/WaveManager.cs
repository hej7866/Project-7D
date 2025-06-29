using System.Collections.Generic;
using UnityEngine;

public class WaveManager : SingleTon<WaveManager>
{
    [SerializeField] private List<GameObject> portals;
    [SerializeField] private GameObject waveZombiePrefab;

    RectInt area = new RectInt(-32, -32, 64, 64);
    Rect area2 = new Rect(-32, -32, 64, 64);
    float spawnTime = 0f;

    void Start()
    {
        TimeManager.Instance.StartWave += HidePortals;
        TimeManager.Instance.EndWave += ShowPortals;
    }

    void Update()
    {
        spawnTime += Time.deltaTime;

        if (TimeManager.Instance.CurrentTimeState == TimeManager.TimeState.Day) return;

        if (spawnTime >= 3)
        {
            WaveZombieSpawn();
            spawnTime = 0f;
        }
    }



    private void HidePortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
        }
    }

    private void WaveZombieSpawn()
    {
        Vector2Int pos = RandomVector2Int();
        float radius = 10f;

        if (!InBase(pos))
        {
            int ran = Random.Range(1, 7);

            for (int i = 1; i < ran; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * radius;
                Vector3 newPos = new Vector3(pos.x + randomOffset.x, 0f, pos.y + randomOffset.y);

                // 랜덤 위치가 베이스 밖인지 체크하는 로직
                Vector2 checkPos = new Vector2(pos.x + randomOffset.x, pos.y + randomOffset.y); 
                if (InBase(checkPos)) continue; // 오버로딩 사용

                Instantiate(waveZombiePrefab, newPos, Quaternion.identity, transform);
            }
        }
    }

    private void ShowPortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(true);
        }
    }


    private Vector2Int RandomVector2Int()
    {
        int x = Random.Range(-256, 256);
        int y = Random.Range(-256, 256);
        return new Vector2Int(x, y);
    }

    public bool InBase(Vector2Int pos)
    {
        bool isInside = area.Contains(pos);
        return isInside;
    }

    public bool InBase(Vector2 pos)
    {
        bool isInside = area2.Contains(pos);
        return isInside;
    }
}
