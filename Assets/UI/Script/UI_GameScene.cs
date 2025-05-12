using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public Button optionButton;
    //public Button statusButton;

    [Header("UI Prefabs (Resources/UI)")]
    
    public string questListName = "UI_QuestList";
    public string healthBarName = "UI_HealthBar";

    private UI_QuestList questList;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    private PlayerManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        if (player == null)
        {
            Debug.LogError("PlayerManager not found in the scene.");
            return;
        }
        player.OnHealthChanged += SetHealth;
        player.OnPlayerDie += ShowFailPopup;
        SetHealth(player.CurrentHealth, player.MaxHealth);

        SetInfo();
    }

    public void Init()
    {
        GameManager.instance.OnStageUpdated -= OnStageUpdated;
        GameManager.instance.OnStageUpdated += OnStageUpdated;

        optionButton.onClick.AddListener(OnClickOptionButton);
        //statusButton.onClick.AddListener(OnClickStatusButton);

        //questList = UIManager.Instance.ShowPopup<UI_QuestList>(questListName);
        //questList.Init();      

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

    //void OnClickStatusButton()
    //{
    //    UI_StatsPopup statusPopup = UIManager.Instance.ShowPopup<UI_StatsPopup>("UI_StatsPopup");
    //    statusPopup.Init();
    //}
    public void SetHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        healthText.text = $"{current}/{max}";
    }

    private void ShowFailPopup()
    {
        var popup = UIManager.Instance.ShowPopup<UI_StageFailPopup>("UI_StageFailPopup");
        popup.Init();
    }
}