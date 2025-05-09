using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action OnStageUpdated;
    
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



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }
}
