using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsPopup : UI_Popup
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI equipmentText;
    public TextMeshProUGUI statsText;

    public Button exitButton;

    public void Init(/*CharacterData data*/)
    {
        // TODO: data�� ������� characterNameText, equipmentText, statsText ������Ʈ
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    void OnClickExitButton()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
