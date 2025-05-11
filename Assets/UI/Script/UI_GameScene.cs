using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public Button optionButton;

    private void Start()
    {
        
    }

    public void Init()
    {
        GameManager.instance.OnStageUpdated -= OnStageUpdated;
        GameManager.instance.OnStageUpdated += OnStageUpdated;

        optionButton.onClick.AddListener(OnClickOptionButton);
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

    }

    void OnStageUpdated()
    {
        stageText.text = "스테이지 " + GameManager.instance.Stage.ToString();
    }

    void OnClickOptionButton()
    {
        UI_OptionPopup optionPopup =  UIManager.Instance.ShowPopup<UI_OptionPopup>("UI_OptionPopup");
        optionPopup.Init();
    }
}
