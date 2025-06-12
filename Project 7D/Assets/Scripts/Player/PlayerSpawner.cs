using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("참조")]
    public GameObject playerPrefab;
    public CinemachineVirtualCamera virtualCamera;

    private GameObject spawnedPlayer;
    [SerializeField] Vector3 spawnPos = new Vector3(0f, 0f, 0f);

    void Start()
    {
        spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        if (virtualCamera != null)
        {
            virtualCamera.Follow = spawnedPlayer.transform;
        }

        ZoneManager.Instance.SetPlayer(spawnedPlayer.transform);
    }


}
