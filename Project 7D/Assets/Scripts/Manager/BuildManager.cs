using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("설치할 프리팹")]
    public GameObject BuildingPrefab;

    [Header("프리뷰 (고스트)")]
    public GameObject PreviewPrefab;
    private GameObject previewInstance;
    private Renderer previewRenderer;

    [Header("설치 관련 설정")]
    public LayerMask GroundLayer;
    public LayerMask PlacementBlockedLayers;
    public float GridSize = 1f;
    public Vector3 BoxSize = new Vector3(1f, 1f, 1f);

    private bool isBuildMode = false;

    void Update()
    {
        // Q 키로 설치 모드 토글
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleBuildMode();
        }

        if (!isBuildMode || previewInstance == null) return;

        // 마우스 위치 기반 스냅
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
        {
            Vector3 snappedPos = GetSnappedPosition(hit.point);
            previewInstance.transform.position = snappedPos;

            bool canBuild = CanPlaceBuilding(snappedPos);
            SetPreviewColor(canBuild ? Color.green : Color.red);

            // 설치 완료 시
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                Instantiate(BuildingPrefab, snappedPos, Quaternion.identity);
                Destroy(previewInstance);
                previewInstance = null;
                isBuildMode = false;
            }
        }
    }

    void ToggleBuildMode()
    {
        if (isBuildMode)
        {
            // 모드 종료
            if (previewInstance != null) Destroy(previewInstance);
            previewInstance = null;
            isBuildMode = false;
        }
        else
        {
            // 모드 시작
            previewInstance = Instantiate(PreviewPrefab);
            previewRenderer = previewInstance.GetComponentInChildren<Renderer>();
            isBuildMode = true;
        }
    }

    Vector3 GetSnappedPosition(Vector3 rawPos)
    {
        float x = Mathf.Round(rawPos.x / GridSize) * GridSize;
        float z = Mathf.Round(rawPos.z / GridSize) * GridSize;
        return new Vector3(x, 0f, z);
    }

    bool CanPlaceBuilding(Vector3 position)
    {
        return !Physics.CheckBox(position, BoxSize / 2f, Quaternion.identity, PlacementBlockedLayers);
    }

    void SetPreviewColor(Color color)
    {
        if (previewRenderer != null)
        {
            Material mat = previewRenderer.material;
            color.a = 0.5f;
            mat.color = color;
        }
    }
}
