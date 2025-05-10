using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FIreBallCIrcle : MonoBehaviour
{
    public Transform centerTarget; // ȸ�� �߽� (����)
    public float rotationSpeed = 90f; // �ʴ� ȸ�� ����
    private List<Transform> bullets = new List<Transform>();

    public void Init(Transform center, List<Transform> fireballs)
    {
        centerTarget = center;
        bullets = fireballs;
    }

    void Update()
    {
        if (centerTarget == null || bullets.Count == 0) return;

        float angle = rotationSpeed * Time.deltaTime;

        foreach (Transform bullet in bullets)
        {
            Vector3 dir = bullet.position - centerTarget.position;
            dir = Quaternion.Euler(0, 0, angle) * dir;
            bullet.position = centerTarget.position + dir;
        }
    }
}
