using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PortalManager : SingleTon<PortalManager>
{
    [System.Serializable]
    public struct PortalConnection
    {
        public GameObject portal;
        public GameObject linkedPortal;
    }

    [Header("Portal Settings")]
    public Vector3[] linkedPortalPos;
    [SerializeField] private List<PortalConnection> portalLinks = new List<PortalConnection>();
    [SerializeField] private GameObject portalContainer;

    [Header("터레인")]
    [SerializeField] private List<Terrain> terrains;

    /// <summary>
    /// 도시 - 숲 - 사막 -  설산 링크드포탈 설치
    /// </summary>
    private int idx = 0;
    public List<GameObject> linkedPortarList;
    void Start()
    {

        foreach (PortalConnection portalLink in portalLinks)
        {
            Terrain terrain = terrains[idx];

            Vector3 pos = linkedPortalPos[idx];
            pos.y = terrain.SampleHeight(linkedPortalPos[idx]);
            GameObject linkedPortalInstance = Instantiate(portalLink.linkedPortal, pos, Quaternion.identity, portalContainer.transform);
            linkedPortarList.Add(linkedPortalInstance);
            idx++;
        }
    }
}
