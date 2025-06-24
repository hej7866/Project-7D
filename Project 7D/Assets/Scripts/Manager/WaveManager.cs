using System.Collections.Generic;
using UnityEngine;

public class WaveManager : SingleTon<WaveManager>
{
    [SerializeField] private List<GameObject> portals;
    [SerializeField] private GameObject waveZombiePrefab;

    RectInt area = new RectInt(0, 0, 64, 64);
    float spawnTime = 0f;

    void Start()
    {
        TimeManager.Instance.StartWave += HidePortals;
        TimeManager.Instance.EndWave += ShowPortals;
    }

    void Update()
    {
        spawnTime += Time.deltaTime;

        // if (TimeManager.Instance.isDayTime) return;

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
        if (!InBase(pos))
        {
            Vector3 newPos = new Vector3(pos.x, 0f, pos.y);
            int ran = Random.Range(1, 7);

            for (int i = 1; i < ran; i++)
            {   
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


    Vector2Int RandomVector2Int()
    {
        int x = Random.Range(-256, 256);
        int y = Random.Range(-256, 256);
        return new Vector2Int(x, y);
    }

    bool InBase(Vector2Int pos)
    {
        bool isInside = area.Contains(pos);
        return isInside;
    }
}
