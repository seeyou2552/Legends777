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
    
    private int stage;
    public int Stage 
    { 
        get { return stage; } 
        set
        {
            stage = value;
            OnStageUpdated?.Invoke();
        }
    }

    private DungeonType dungeonType;
    public DungeonType DungeonType
    {
        get { return dungeonType; }
        set
        {
            dungeonType = value;
            if (dungeonType == DungeonType.Monster)
            {
                Debug.Log("Monster changed");
                OnDungeonTypeMonsterUpdated?.Invoke();
            }
            else if (dungeonType == DungeonType.Boss)
            {
                Debug.Log("Boss changed");
                OnDungeonTypeBossUpdated?.Invoke();
            }
        }
    }



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }
}
