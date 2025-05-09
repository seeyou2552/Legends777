using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    // // 카메라가 넘어가지 못할 영역
    // public TilemapCollider2D cameraBounds;

    private float halfHeight;
    private float halfWidth;

    public float minCostomX;
    public float maxCostomX;
    public float minCostomY;
    public float maxCostomY;

    private IEnumerator Start()
    {
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
        while (target == null)
        {
            GameObject obj = GameObject.FindWithTag("Player");
            if (obj != null)
            {
                target = obj.transform;
                yield break;
            }

            yield return null;
        }
    }

    private void LateUpdate()
    {
        // if (target == null || cameraBounds == null) return;
        if (target == null) return;
        Vector3 targetPos = target.position;
        // 벽 타일맵을 벗어나지 못하도록 설정
        // float minX = cameraBounds.bounds.min.x + halfWidth;
        // float maxX = cameraBounds.bounds.max.x - halfWidth;
        // float minY = cameraBounds.bounds.min.y + halfHeight;
        // float maxY = cameraBounds.bounds.max.y - halfHeight;

        float minX = minCostomX + halfWidth; // -12.5
        float maxX = maxCostomX - halfWidth; // 12.5
        float minY = minCostomY + halfHeight;  // -8
        float maxY = maxCostomY - halfHeight;  // 25

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, -10f);
    }
}
