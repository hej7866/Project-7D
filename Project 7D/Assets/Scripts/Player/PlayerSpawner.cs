using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("참조")]
    public GameObject PlayerPrefab;
    public CinemachineVirtualCamera VirtualCamera;

    private GameObject spawnedPlayer;
    [SerializeField] Vector3 spawnPos = new Vector3(0f, 0f, 0f);

    void Start()
    {
        spawnedPlayer = Instantiate(PlayerPrefab, spawnPos, Quaternion.identity);

        if (VirtualCamera != null)
        {
            VirtualCamera.Follow = spawnedPlayer.transform;
        }

        ZoneManager.Instance.SetPlayer(spawnedPlayer.transform);
    }


}
