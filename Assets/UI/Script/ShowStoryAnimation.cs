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
        { "��...���� �����?��\n������ ���ٴ�, ������ ����.",
        "�ƹ��͵� ��ﳪ�� �ʾ�\n ������... �̴�� ������ ���� ���� ����",
        "���𰡡� ���� �̲��� �ִ�.",
        "���ǡ� ��?",
        "��������, �� �� �ʸӿ� ���� �ִ�." };

    //[SerializeField] private float typeSpeed = 0.005f; // Ÿ���� �ӵ�

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        SpriteRenderer sr = Ima.GetComponent<SpriteRenderer>();

        for(int i = 0; i < storyLines.Length; i++)
        {
            sr.sprite = StoryImages[i];

            // �̹��� �ʱ�ȭ
            Color imageColor = sr.color;
            imageColor.a = 0f;
            sr.color = imageColor;

            // �ؽ�Ʈ�� �ʱ�ȭ
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

            // ���� �ð�
            yield return new WaitForSeconds(0.5f);

            // ���̵� �ƿ�
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
            yield return new WaitForSeconds(0.01f); ;
        }
    }
}
