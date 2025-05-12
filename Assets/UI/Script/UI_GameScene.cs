using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public Button optionButton;
    public Button statusButton;

    [Header("UI Prefabs (Resources/UI)")]
    
    public string questListName = "UI_QuestList";
    public string healthBarName = "UI_HealthBar";

    private UI_QuestList questList;
    private UI_HealthBar healthBar;

    private void Start()
    {
        SetInfo();
    }

    public void Init()
    {
        GameManager.instance.OnStageUpdated -= OnStageUpdated;
        GameManager.instance.OnStageUpdated += OnStageUpdated;

        optionButton.onClick.AddListener(OnClickOptionButton);
        statusButton.onClick.AddListener(OnClickStatusButton);

        questList = UIManager.Instance.ShowPopup<UI_QuestList>(questListName);
        questList.Init();

        healthBar = UIManager.Instance.ShowPopup<UI_HealthBar>(healthBarName);

        //var stat = PlayerStat.Instance;
        //healthBar.SetHealth(stat.CurrentHealth, statMaxHealth: stat.MaxHealth);
        //stat.OnHealthChanged += (cur, max) =>
        //{
        //    healthBar.SetHealth(cur, max);
        //};

    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        OnStageUpdated();
        // TODO: 다른 UI 요소 리프레시 호출
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

    void OnClickStatusButton()
    {
        UI_StatsPopup statusPopup = UIManager.Instance.ShowPopup<UI_StatsPopup>("UI_StatsPopup");
        statusPopup.Init();
    }
}
