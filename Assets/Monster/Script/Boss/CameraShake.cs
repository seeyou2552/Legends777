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

    public IEnumerator Shake(float duration, float magnitude) // ��鸲 �ð��� ����
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f; // �󸶳� �ð��� �귶���� �����ϴ� ����

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // X, Y ������ ������ �� ���� magnitude�� ��鸲 ���� ���� ��: 0.2f�� - 0.2 ~0.2 ���� ������ ��鸲
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
