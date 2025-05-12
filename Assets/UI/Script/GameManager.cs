using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                isStageClear = false;
                currentWaveIndex = 0;
                StartMonsterStage();
            }
            else if (dungeonType == DungeonType.Boss)
            {
                isStageClear = false;
                OnDungeonTypeBossUpdated?.Invoke();
            }
            else
            {
                isStageClear = true;
            }
        }
    }

    [SerializeField] private bool isStageClear;
    public bool IsStageClear { get { return isStageClear; } }

    private void Awake()
    {
        instance = this;
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
            isStageClear = true;
            return;
        }

        MonsterManager.Instance.StartWave(currentWaveIndex);
    }

    public void EndOfWave()
    {
        Debug.Log("End OF Wave");
        StartNextWave();
    }
}
