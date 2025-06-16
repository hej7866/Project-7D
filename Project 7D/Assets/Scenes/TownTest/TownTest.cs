using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTest : MonoBehaviour
{
    [SerializeField] private GameObject[] townPrefabs;
    [SerializeField] private GameObject parent;

    void Start()
    {
        BuildTown();
    }

    void BuildTown()
    {
        for (int x = 1; x <= 4; x += 3)
        {
            for (int y = 1; y <= 4; y++)
            {
                Vector3 pos = new Vector3(x * 100, 0, y * 100);
                Instantiate(townPrefabs[0], pos, Quaternion.identity, parent.transform);
            }
        }
        
    }
}
