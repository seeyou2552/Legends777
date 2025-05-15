using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public SpriteRenderer fadeImage;

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
        float duration = 13; // 전체 걸리는 시간
        float time = 0f;

        Vector3 startScale = new Vector3(22f, 0f);
        Vector3 endScale = new Vector3(22f, 13f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time;

            fadeImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // 보정: 정확히 마지막 크기 설정
        fadeImage.transform.localScale = endScale;
    }
}
