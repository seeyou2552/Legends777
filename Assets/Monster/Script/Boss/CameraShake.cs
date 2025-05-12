using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    

    private void Awake()
    {
        instance = this;
    }

    public void StartShake()
    {
        StartCoroutine(Shake(5f, 0.2f));
    }

    public IEnumerator Shake(float duration, float magnitude) // 흔들림 시간과 강도
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f; // 얼마나 시간이 흘렀는지 추적하는 변수

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // X, Y 축으로 무작위 값 생성 magnitude로 흔들림 강도 조절 예: 0.2f면 - 0.2 ~0.2 사이 값으로 흔들림
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
