using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    

    void OnClickExitButton()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
