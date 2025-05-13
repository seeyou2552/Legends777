using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public Button optionButton;
    //public Button statusButton;
    public TextMeshProUGUI goldText;

    [Header("Skill Icons")]
    public RectTransform skillsContainer;
    public GameObject skillIconPrefab;

    private Dictionary<string, TextMeshProUGUI> skillIconMap = new Dictionary<string, TextMeshProUGUI>();

    private GridLayoutGroup grid;
    private SkillManager skillManager;


    [Header("UI Prefabs (Resources/UI)")]
    
    public string questListName = "UI_QuestList";
    public string healthBarName = "UI_HealthBar";

    private UI_QuestList questList;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    private PlayerManager player;
    private PlayerController playerController;

    [Header("Dash Icon")]
    public TextMeshProUGUI coolTimeText;
    public Image dashIcon;
    public Image dashImage;
    public Animator dashIconAnim;

    private void Awake()
    {
        grid = skillsContainer.GetComponent<GridLayoutGroup>();
        skillManager = FindObjectOfType<SkillManager>();

        GameManager.instance.OnSkillUpgraded += OnSkillUpgraded;
        PlayerController.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(PlayerController.Instance.Gold);
    }
    
    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        playerController = player.GetComponent<PlayerController>();
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
        // TODO: ?¤ë¥¸ UI ?”ì†Œ ë¦¬í”„?ˆì‹œ ?¸ì¶œ
    }

    void OnStageUpdated()
    {
        stageText.text = "½ºÅ×ÀÌÁö " + GameManager.instance.Stage.ToString();
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

    private void OnDestroy()
    {
        GameManager.instance.OnSkillUpgraded -= OnSkillUpgraded;
    }

    private void OnSkillUpgraded(SkillOption option)
    {
        AddSkillIcon(option);
        UpdateGridConstraint();
    }

    private void AddSkillIcon(SkillOption option)
    {
        string key = option.Name;

        string value = GetSkillValue(key);

        if(skillIconMap.TryGetValue(key, out var existingText))
        {
            existingText.text = $"{key}: \n{value}";
        }
        else
        {
            var icon = Instantiate(skillIconPrefab, skillsContainer, false);

            var rt = icon.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;

            var iconImage = icon.transform.Find("Skillicon/SkillImage")?.GetComponent<Image>();
            iconImage.sprite = option.Icon;

            var iconText = icon.GetComponentInChildren<TextMeshProUGUI>();
            iconText.text = $"{key}: \n{value}";

            skillIconMap[key] = iconText;
        }


    }

    private string GetSkillValue(string key)
    {
        switch (key)
        {
            case "shootSpeed":
                return skillManager.shootSpeed.ToString();
            case "arrowCount":
                return skillManager.arrowCount.ToString();
            case "addGhost":
                return skillManager.addGhost ? "True" : "False";
            case "addBomb":
                return skillManager.addBomb.ToString();
            case "addPenetrates":
                return skillManager.addPenetrates ? "True" : "False";
            case "addSpread":
                return skillManager.addSpread.ToString();
            case "addChase":
                return skillManager.addChase ? "True" : "False";
            default:
                return "";
        }
    }

    private void UpdateGridConstraint()
    {
        float width = skillsContainer.rect.width;
        float cellAndSpacing = grid.cellSize.x + grid.spacing.x;

        int maxColumns = Mathf.Max(1, Mathf.FloorToInt((width + grid.spacing.x) / cellAndSpacing));

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = maxColumns;
    }

    private void UpdateGoldUI(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    void Update()
    {
        if(playerController.dashCool > 0f) // ì¿¨í???????
        {
            coolTimeText.text = playerController.dashCool.ToString("N1");
            dashIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            dashImage.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            dashIconAnim.Play("DashIconState", -1, 0f);
        }
        else if(playerController.dashCool < 0) // ì¿¨ì´ ?ë‚¬????
        {
            coolTimeText.text = "";
            dashIcon.color = new Color(1f, 1f, 1f, 1f);
            dashImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }

}