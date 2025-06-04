using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("참조")]
    public GameObject playerPrefab;
    public CinemachineVirtualCamera virtualCamera;

    [Header("맵 정보")]
    public int MapWidth = 2048;
    public int MapHeight = 2048;

    private GameObject spawnedPlayer;

    void Start()
    {
        Vector3 spawnPos = new Vector3(MapWidth / 2f, 0f, MapHeight / 2f);
        spawnedPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        if (virtualCamera != null)
        {
            virtualCamera.Follow = spawnedPlayer.transform;
        }

        ZoneManager.Instance.SetPlayer(spawnedPlayer.transform);
    }


}
