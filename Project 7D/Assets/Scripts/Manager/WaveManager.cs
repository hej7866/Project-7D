using System.Collections.Generic;
using UnityEngine;

public class WaveManager : SingleTon<WaveManager>
{
    [SerializeField] private List<GameObject> portals;

    void Start()
    {
        TimeManager.Instance.StartWave += HidePortals;
        TimeManager.Instance.EndWave += ShowPortals;
    }

    public void HidePortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
        }
    }

    public void ShowPortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(true);
        }
    }
}
