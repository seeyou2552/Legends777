using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPopup : MonoBehaviour
{
    public static EndingPopup Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI killCountText;

    private void Start()
    {
        Instance = this;
    }

    public void ShowScore()
    {
        int killCount = GameManager.instance.KillCount;
        killCountText.text = $"Kill Count: {killCount}";
    }
    public void RestartButton()
    {
        SceneManager.LoadScene("Legends777");
    }
    public void GoMenuButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
