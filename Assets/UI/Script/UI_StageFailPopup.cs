using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_StageFailPopup : UI_Popup
{
    public Button retryButton;
    public Button titleButton;

    public void Init()
    {
        retryButton.onClick.AddListener(() =>
        {
            // ���� �κ�(���� ��)�� �̵�
            SceneManager.LoadScene("Legends777");
        });
        titleButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }
}
