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

    private void OnStartButtonClicked()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Legends777");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

}
