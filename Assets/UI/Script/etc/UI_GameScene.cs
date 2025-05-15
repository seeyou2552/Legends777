using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Potion Status Form")]
    public TextMeshProUGUI potionStatusText;
    public Animator potionStatusAnim;

    [Header("Tutorial")]
    public GameObject tutorialPrefab;
    private GameObject tutorialInstance;

    private void Awake()
    {
        grid = skillsContainer.GetComponent<GridLayoutGroup>();
        skillManager = FindObjectOfType<SkillManager>();

        GameManager.instance.OnSkillUpgraded += OnSkillUpgraded;
        PlayerController.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(PlayerController.Instance.Gold);

        GameManager.instance.OnTutorialUpdated += ShowTutorialUI;
        GameManager.instance.OnDungeonTypeMonsterUpdated += HideTutorialUI;
        GameManager.instance.OnDungeonTypeBossUpdated += HideTutorialUI;
        GameManager.instance.OnDungeonTypeDefaultUpdated += HideTutorialUI;
        if (GameManager.instance.DungeonType == DungeonType.Lobby)
            ShowTutorialUI();

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
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        OnStageUpdated();
        // TODO: ?¤ë¥¸ UI ??�ì†�?ë¦¬í?��??ì?��??¸ì¶œ
    }

    void OnStageUpdated()
    {
        stageText.text = "Stage " + GameManager.instance.Stage.ToString();
    }

    void OnClickOptionButton()
    {
        UI_OptionPopup optionPopup = UIManager.Instance.ShowPopup<UI_OptionPopup>("UI_OptionPopup");
        optionPopup.Init();
    }
 
    public void SetHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        healthText.text = $"{current}/{max}";
    }

    private void ShowFailPopup()
    {
        SceneManager.LoadScene("FailEnding");
    }

    private void OnDestroy()
    {
        GameManager.instance.OnSkillUpgraded -= OnSkillUpgraded;
        GameManager.instance.OnTutorialUpdated -= ShowTutorialUI;
        GameManager.instance.OnDungeonTypeMonsterUpdated -= HideTutorialUI;
        GameManager.instance.OnDungeonTypeBossUpdated -= HideTutorialUI;
        GameManager.instance.OnDungeonTypeDefaultUpdated -= HideTutorialUI;
    }

    private void OnSkillUpgraded(SkillOption option)
    {
        AddSkillIcon(option);
        UpdateGridConstraint();
    }

    private void AddSkillIcon(SkillOption option)
    {
        string key = option.Name;

        string value = GetSkillValue(option.Id);

        if (skillIconMap.TryGetValue(key, out var existingText))
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
            case "addFreeze":
                return skillManager.addFreeze ? "True" : "False";
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
        if (playerController.dashCool > 0f) // 쿨�???????
        {
            coolTimeText.text = playerController.dashCool.ToString("N1");
            dashIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            dashImage.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            dashIconAnim.Play("DashIconState", -1, 0f);
        }
        else if (playerController.dashCool < 0) // 쿨이 ?�났????
        {
            coolTimeText.text = "";
            dashIcon.color = new Color(1f, 1f, 1f, 1f);
            dashImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void SetStatus(string name, float status)
    {
        if (name.StartsWith("HP_Potion"))
        {
            potionStatusText.text = "체력이" + status.ToString() + " 만큼 회복되었습니다.";
        }
        else if (name.StartsWith("Power_Potion"))
        {
            potionStatusText.text = "공격력이 " + status.ToString() + " 만큼 상승하였습니다.";
        }
        else if (name.StartsWith("AttackSpeed_Potion"))
        {
            potionStatusText.text = "공격 속도가 " + status.ToString() + " 만큼 상승하였습니다.";
        }

        potionStatusText.gameObject.SetActive(true);
        StartCoroutine(OutPutStatus());
    }

    IEnumerator OutPutStatus()
    {
        potionStatusAnim.Play("StatusForm", -1, 0f);
        yield return new WaitForSeconds(1f);
        potionStatusText.gameObject.SetActive(false);
    }

    private void ShowTutorialUI()
    {
        if (tutorialInstance == null && tutorialPrefab != null)
        {
            tutorialInstance = Instantiate(tutorialPrefab);
            tutorialInstance.transform.SetParent(transform);
        }
    }

    private void HideTutorialUI()
    {
        if (tutorialInstance != null)
        {
            Destroy(tutorialInstance);
            tutorialInstance = null;
        }
    }


}