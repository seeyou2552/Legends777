﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static bool HasInstance => _instance != null;
    public static T TryGetInstance() => HasInstance ? _instance : null;
    public static T Current => _instance;

    /// <summary>
    /// 싱글톤 디자인 패턴
    /// </summary>
    /// <value>인스턴스</value>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name + "_AutoCreated";
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Awake에서 인스턴스를 초기화합니다. 만약 awake를 override해서 사용해야 한다면 base.Awake()를 호출해야 합니다.
    /// </summary>
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    /// <summary>
    /// 싱글톤을 초기화합니다.
    /// </summary>
    protected virtual void InitializeSingleton()
    {
        //게임이 실행중이 아니라면 종료합니다.
        if (!Application.isPlaying)
        {
            return;
        }

        _instance = this as T;
    }
}