using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFailEnding : MonoBehaviour
{
    public SpriteRenderer fadeImage;
    public Image FAILImage;
    public Image MenuImage;
    public Image RestartImage;

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FadeIn(new[] { fadeImage}));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeIn2(new[] {FAILImage }));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeIn2(new[] { MenuImage, RestartImage}));
        EndingPopup.Instance.ShowScore();
    }

    private IEnumerator FadeIn(SpriteRenderer[] sprites)
    {
        // 모든 스프라이트 초기 투명도 설정
        foreach (var sprite in sprites)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c;
        }

        // 페이드인
        for (int i = 0; i <= 10; i++)
        {
            foreach (var sprite in sprites)
            {
                Color c = sprite.color;
                c.a = i * 0.1f;
                sprite.color = c;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FadeIn2(Image[] sprites)
    {
        // 모든 스프라이트 초기 투명도 설정
        foreach (var sprite in sprites)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c;
        }

        // 페이드인
        for (int i = 0; i <= 10; i++)
        {
            foreach (var sprite in sprites)
            {
                Color c = sprite.color;
                c.a = i * 0.1f;
                sprite.color = c;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
