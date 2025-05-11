using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FIreBallCIrcle : MonoBehaviour
{
    public Transform boss; // ������ ��ġ
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float radius = 2f; // ���� ������ ������

    private float currentAngle; // ���̾�� ���� ����

    

    void Update()
    {

        // ���� ������ �������� ���� �������� ȸ���ϵ��� �Ѵ�.
        currentAngle += rotationSpeed * Time.deltaTime; // ȸ�� �ӵ���ŭ ������ ������Ų��.

        if (currentAngle >= 360f) currentAngle -= 360f; // ������ 360���� �Ѿ�� 0���� ����

        // ȸ���ϴ� ��ġ�� ���
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;

        // ���� ������ ��ġ�� �̵�
        transform.position = new Vector3(boss.position.x + x, boss.position.y + y, boss.position.z);
    }
    private void OnEnable()
    {
        if (BossManager.instance == null) return;
        boss = BossManager.instance.transform;
    }
}
