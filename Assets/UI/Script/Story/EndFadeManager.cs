using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndFadeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    private string[] storyLines =
        { "“이곳은...?","아무것도 모르겠다...", "하지만... ","느껴진다...","저 성으로 가야 한다는 것을..." };

    public SpriteRenderer fadeImage;
    public Image ClearImage;
    public Image MenuImage;
    public Image RestartImage;

    public IEnumerator Start()
    {
        yield return StartCoroutine(FadeIn(new[] { fadeImage }));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowStory());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeIn2(new[] { ClearImage, MenuImage, RestartImage }));
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

    private IEnumerator ShowStory()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < storyLines.Length; i++)
        {

            // 텍스트도 초기화
            Text.text = "";
            Color textColor = Text.color;
            textColor.a = 1f;
            Text.color = textColor;

            yield return StartCoroutine(TypeText(storyLines[i]));

            // 유지 시간
            yield return new WaitForSeconds(0.5f);

            // 페이드 아웃
            for (int j = 0; j < 10; j++)
            {
                textColor.a -= 0.1f;
                Text.color = textColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    

    private IEnumerator TypeText(string line)
    {
        Text.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            Text.text += line[i];
            yield return new WaitForSeconds(0.1f); ;
        }
    }
}
