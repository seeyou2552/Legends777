using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action OnStageUpdated;
    public Action OnDungeonTypeMonsterUpdated;
    public Action OnDungeonTypeBossUpdated;
    public Action OnDungeonTypeDefaultUpdated;
    public Action<String> OnSkillUpgraded;
    public Action OnTutorialUpdated;

    public int KillCount { get; set; } = 0;
    private bool OnStageResult = false;

    [SerializeField] private int maxStage = 8;
    [SerializeField] private int stage;

    public int Stage
    {
        get { return stage; }
        set
        {
            if (value > maxStage)
            {
                value = maxStage;
            }
            else
            {
                stage = value;
            }

            OnStageUpdated?.Invoke();
        }
    }

    [SerializeField] private DungeonType dungeonType;
    public DungeonType DungeonType
    {
        get { return dungeonType; }
        set
        {
            dungeonType = value;
            if (dungeonType == DungeonType.Monster)
            {
                OnStageResult = false;
                IsStageClear = false;
                currentWaveIndex = 0;
                StartMonsterStage();
            }
            else if (dungeonType == DungeonType.Boss)
            {
                OnStageResult = false;
                IsStageClear = false;
                OnDungeonTypeBossUpdated?.Invoke();
            }
            else if (dungeonType == DungeonType.Lobby)
            {
                IsStageClear = true;
                OnTutorialUpdated?.Invoke();
            }
            else
            {
                IsStageClear = true;
                OnDungeonTypeDefaultUpdated?.Invoke();
            }
            
        }
    }

    public bool IsStageClear;

    private void Awake()
    {
        instance = this;
        OnDungeonTypeBossUpdated += () => StartCoroutine(SubscribeToBossDeath());
        OnDungeonTypeDefaultUpdated += () => StartCoroutine(SubscribeToDefaultClear());    
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowStageResult();
        }
    }

    int currentWaveIndex;

    void StartMonsterStage()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex > stage && !OnStageResult && dungeonType != DungeonType.Boss)
        {
            if (stage != maxStage)
            {
                OnStageResult = true;
                ShowStageResult();
                Debug.Log("Skill Select Popup");
            }
            else if (stage == maxStage && AreAllMonstersDefeated())
            {
                ShowClearResult();
            }
            return;
        }

        MonsterManager.Instance.StartWave(currentWaveIndex);
    }

    public void EndOfWave()
    {

        StartNextWave();
    }

    void ShowStageResult()
    {

        var popup = UIManager.Instance.ShowPopup<UI_StageResult>("UI_StageResult");
        popup.Init();

    }

    void ShowClearResult()
    {

        var popup = UIManager.Instance.ShowPopup<UI_ClearResult>("UI_ClearResult");
        popup.Init();

    }

    private IEnumerator SubscribeToBossDeath()
    {
        yield return null;
        if (BossManager.instance != null)
        {
            BossManager.instance.Ondead -= ShowStageResult;
            BossManager.instance.Ondead -= ShowStageResult;
            if (stage != maxStage)
            {
                BossManager.instance.Ondead += ShowStageResult;
                Debug.Log("Skill Select Popup");
            }
            else if (stage == maxStage)
            {
                BossManager.instance.Ondead += ShowClearResult;
            }
        }
    }
    private IEnumerator SubscribeToDefaultClear()
    {
        yield return null;
        if(stage == maxStage)
        {
            ShowClearResult();
        }

    }

    private bool AreAllMonstersDefeated()
    {
        return MonsterManager.Instance.activeMonsters.Count == 0;
    }

}
