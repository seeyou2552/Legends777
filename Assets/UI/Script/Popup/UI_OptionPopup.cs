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
        bgmSlider.value = SoundManager.Instance.bgmVolume;
        sfxSlider.value = SoundManager.Instance.sfxVolume;

        bgmSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.SetVolume(value, SoundManager.Instance.sfxVolume);
        });

        sfxSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.SetVolume(SoundManager.Instance.bgmVolume, value);
        });

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
