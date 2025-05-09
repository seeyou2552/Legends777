using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab;

    private void Start()
    {
        GameObject bossInstance = Instantiate(bossPrefab);
        bossInstance.SetActive(true); // 일단 비활성화
    }
}
