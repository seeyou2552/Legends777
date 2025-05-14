using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ShowStoryAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] StoryImages;
    [SerializeField] private GameObject Ima;
    [SerializeField] private TextMeshProUGUI StoryText;
    private string[] storyLines = 
        { "“...여긴 어디지?”\n차가운 돌바닥, 축축한 공기.",
        "아무것도 기억나지 않아\n 하지만... 이대로 가만히 있을 수는 없어",
        "무언가… 나를 이끌고 있다.",
        "저건… 빛?",
        "느껴진다, 저 문 너머에 무언가 있다." };

    //[SerializeField] private float typeSpeed = 0.005f; // 타이핑 속도

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        SpriteRenderer sr = Ima.GetComponent<SpriteRenderer>();

        for(int i = 0; i < storyLines.Length; i++)
        {
            sr.sprite = StoryImages[i];

            // 이미지 초기화
            Color imageColor = sr.color;
            imageColor.a = 0f;
            sr.color = imageColor;

            // 텍스트도 초기화
            StoryText.text = "";
            Color textColor = StoryText.color;
            textColor.a = 1f;
            StoryText.color = textColor;

            for (int j = 0; j < 10; j++)
            {
                imageColor.a += 0.1f;
                sr.color = imageColor;

                yield return new WaitForSeconds(0.1f);
            }
            yield return StartCoroutine(TypeText(storyLines[i]));

            // 유지 시간
            yield return new WaitForSeconds(0.5f);

            // 페이드 아웃
            for (int j = 0; j < 10; j++)
            {
                imageColor.a -= 0.1f;
                sr.color = imageColor;
                textColor.a -= 0.1f;
                StoryText.color = textColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
        FadeManager.instance.StartFade();
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Legends777");
    }

    private IEnumerator TypeText(string line)
    {
        StoryText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            StoryText.text += line[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
