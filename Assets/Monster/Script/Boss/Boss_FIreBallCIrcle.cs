using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FIreBallCIrcle : MonoBehaviour
{
    public Transform boss; // 보스의 위치
    public float rotationSpeed = 50f; // 회전 속도
    public float radius = 2f; // 보스 주위의 반지름

    private float currentAngle; // 파이어볼의 현재 각도

    

    void Update()
    {

        // 현재 각도를 기준으로 보스 주위에서 회전하도록 한다.
        currentAngle += rotationSpeed * Time.deltaTime; // 회전 속도만큼 각도를 증가시킨다.

        if (currentAngle >= 360f) currentAngle -= 360f; // 각도가 360도를 넘어가면 0으로 리셋

        // 회전하는 위치를 계산
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;

        // 보스 주위의 위치로 이동
        transform.position = new Vector3(boss.position.x + x, boss.position.y + y, boss.position.z);
    }
    private void OnEnable()
    {
        if (BossManager.instance == null) return;
        boss = BossManager.instance.transform;
    }
}
