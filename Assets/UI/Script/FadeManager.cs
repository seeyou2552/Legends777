using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public SpriteRenderer fadeImage;

    float duration = 5; // 전체 걸리는 시간

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
        float speedUpTime = duration * 0.5f; // 절반 시점 이후에 가속

        Vector3 startScale = new Vector3(22f, 0f);
        Vector3 endScale = new Vector3(22f, 13f);

        while (time < duration)
        {
            time += Time.deltaTime;

            float t;
            if (time < speedUpTime)
            {
                // 초반: 느리게 진행 (선형 보간 그대로)
                t = time / duration;
            }
            else
            {
                // 후반: 가속 효과 주기 (t를 제곱해서 빠르게 증가시킴)
                float fastTime = (time - speedUpTime) / (duration - speedUpTime);
                t = Mathf.Lerp(speedUpTime / duration, 1f, fastTime * fastTime);
            }

            fadeImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        fadeImage.transform.localScale = endScale; // 마지막 보정
    }
}
