using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalType
{
    Portal,
    LinkedPortal,
}

public class Teleport : MonoBehaviour
{
    private BoxCollider boxCollider;

    [Header("텔레포트 설정")]
    [SerializeField] private BiomeType biome;
    [SerializeField] private PortalType portalType;

    private float onTriggerTime = 0f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        onTriggerTime += Time.deltaTime;

        if (onTriggerTime >= 3f)
        {
            if (portalType == PortalType.LinkedPortal)
            {
                other.transform.position = new Vector3(0, 0, 0);
            }
            onTriggerTime = 0f;  
        }
    }

    void OnTriggerExit(Collider other)
    {
        onTriggerTime = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (portalType == PortalType.Portal)
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
}
