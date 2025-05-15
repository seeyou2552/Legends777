using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;

    private void Awake()
    {
        StartButton.onClick.AddListener(OnStartButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
        }
    }

    private void OnStartButtonClicked()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("ShowStory");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

}
