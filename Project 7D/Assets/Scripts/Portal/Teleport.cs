using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField] private BiomeType biome;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (biome)
        {
            case BiomeType.City:
                other.transform.position = PortalManager.Instance.linkedPortarList[0].transform.position; 
                break;
            case BiomeType.Forest:
                other.transform.position = PortalManager.Instance.linkedPortarList[1].transform.position;
                break;
            case BiomeType.Desert:
                other.transform.position = PortalManager.Instance.linkedPortarList[2].transform.position;
                break;
            case BiomeType.Snow:
                other.transform.position = PortalManager.Instance.linkedPortarList[3].transform.position;
                break;
        }
    }
}
