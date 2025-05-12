using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_OptionPopup : UI_Popup
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button gameExitButton;
    public Button restartButton;
    public Button exitButton;

    public void Init()
    {
        //bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        //sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        gameExitButton.onClick.AddListener(OnClickGameExitButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    

    void OnClickExitButton()
    {
        UIManager.Instance.ClosePopupUI(this);
    }

    void OnClickGameExitButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
