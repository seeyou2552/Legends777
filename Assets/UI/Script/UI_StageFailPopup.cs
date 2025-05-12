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
            // 던전 로비(같은 씬)로 이동
            SceneManager.LoadScene("Legends777");
        });
        titleButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }
}
