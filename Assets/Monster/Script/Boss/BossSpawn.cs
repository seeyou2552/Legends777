using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab;

    private void Start()
    {
        GameManager.instance.OnDungeonTypeBossUpdated += SpawnBoss;
    }

    public void SpawnBoss()
    {
        Instantiate(bossPrefab);
    }
}
