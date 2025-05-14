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
        // TODO: data를 기반으로 characterNameText, equipmentText, statsText 업데이트
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    void OnClickExitButton()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
