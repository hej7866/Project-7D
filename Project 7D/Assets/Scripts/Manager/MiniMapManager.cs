using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float height = 20f;

    IEnumerator Start()
    {
        yield return null; // 한 프레임 대기
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = target.position;
        newPos.y += height;

        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // ← Y축 회전 제거
    }
}
