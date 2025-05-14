using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public SpriteRenderer fadeImage;

    float duration = 5; // ��ü �ɸ��� �ð�

    private void Awake()
    {
        instance = this;
    }

    public void StartFade()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;
        float speedUpTime = duration * 0.5f; // ���� ���� ���Ŀ� ����

        Vector3 startScale = new Vector3(22f, 0f);
        Vector3 endScale = new Vector3(22f, 13f);

        while (time < duration)
        {
            time += Time.deltaTime;

            float t;
            if (time < speedUpTime)
            {
                // �ʹ�: ������ ���� (���� ���� �״��)
                t = time / duration;
            }
            else
            {
                // �Ĺ�: ���� ȿ�� �ֱ� (t�� �����ؼ� ������ ������Ŵ)
                float fastTime = (time - speedUpTime) / (duration - speedUpTime);
                t = Mathf.Lerp(speedUpTime / duration, 1f, fastTime * fastTime);
            }

            fadeImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        fadeImage.transform.localScale = endScale; // ������ ����
    }
}
