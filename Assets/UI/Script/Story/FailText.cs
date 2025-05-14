using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    private string[] storyLines =
        { "��...�̷��� �״°ǰ�","�̰��� ����ΰɱ�..." };

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < storyLines.Length; i++)
        {

            // �ؽ�Ʈ�� �ʱ�ȭ
            Text.text = "";
            Color textColor = Text.color;
            textColor.a = 1f;
            Text.color = textColor;

            yield return StartCoroutine(TypeText(storyLines[i]));

            // ���� �ð�
            yield return new WaitForSeconds(0.5f);

            // ���̵� �ƿ�
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
