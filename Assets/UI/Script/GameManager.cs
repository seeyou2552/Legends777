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

    [SerializeField] private int stage;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;
            if (stage == 8)
            {
                ShowStageResult();
                Debug.Log("Game Clear UI Popup");
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
                IsStageClear = false;
                currentWaveIndex = 0;
                StartMonsterStage();
            }
            else if (dungeonType == DungeonType.Boss)
            {
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
        if (currentWaveIndex > stage)
        {
            ShowStageResult();
            Debug.Log("Skill Select Popup");

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

    private IEnumerator SubscribeToBossDeath()
    {
        yield return null;
        if (BossManager.instance != null)
        {
            BossManager.instance.Ondead += ShowStageResult;
        }
    }
}