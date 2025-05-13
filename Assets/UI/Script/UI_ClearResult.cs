using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ClearResult : UI_Popup
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI killCountText;

    public void Init()
    {
        
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        exitButton.onClick.AddListener(OnExitButtonClicked);
        // Display the kill count
        int killCount = GameManager.instance.KillCount;
        killCountText.text = $"Kill Count: {killCount}";
    }


    void OnExitButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
