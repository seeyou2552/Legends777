using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action OnStageUpdated;
    public Action OnDungeonTypeMonsterUpdated;
    public Action OnDungeonTypeBossUpdated;

    public int KillCount { get; set; } = 0;
    private bool OnStageResult = false;

    [SerializeField] private int maxStage = 8;
    [SerializeField] private int stage;

    public int Stage
    {
        get { return stage; }
        set
        {
            if(value > maxStage)
            {
                value = maxStage;
            }
            else
            {
              stage = value;
            }

            if (stage == maxStage)
            {
                if (dungeonType != DungeonType.Monster && dungeonType != DungeonType.Boss)
                {
 
                    ShowClearResult();
                    Debug.Log("Game Clear UI Popup");
                }
                else if (dungeonType == DungeonType.Monster || dungeonType == DungeonType.Boss)
                {

                    if (AreAllMonstersDefeated() && IsBossDefeated())
                    {
                        ShowClearResult();
                        Debug.Log("No Clear UI Popup");
                    }
                    else
                    {
                        Debug.Log("Monsters or Boss still alive");
                    }
                }
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
            else
            {
                IsStageClear = true;
            }
        }
    }

    public bool IsStageClear;

    private void Awake()
    {
        instance = this;
        OnDungeonTypeBossUpdated += () => StartCoroutine(SubscribeToBossDeath());

    }

    private void Start()
    {

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
            if (stage != maxStage)
            {
                BossManager.instance.Ondead += ShowStageResult;
                Debug.Log("Skill Select Popup");
            }
        }
    }

    private bool AreAllMonstersDefeated()
    {
        return MonsterManager.Instance.activeMonsters.Count == 0;
    }

    // 보스가 처치되었는지 확인하는 메서드
    private bool IsBossDefeated()
    {
        return BossManager.FindObjectOfType<BossManager>() == null;
    }
}
