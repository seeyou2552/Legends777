using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject bossHealthBar;

    private void Start()
    {
        GameManager.instance.OnDungeonTypeBossUpdated += SpawnBoss;
    }

    public void SpawnBoss()
    {
        GameObject bossGo = Instantiate(bossPrefab);

        BossManager bossMgr = bossGo.GetComponent<BossManager>();
        GameObject healthBarPrefab = Instantiate(bossHealthBar);
        
        UI_BossHealthBar healthBar = healthBarPrefab.GetComponent<UI_BossHealthBar>();
        healthBar.Init(bossMgr);
    }
}
