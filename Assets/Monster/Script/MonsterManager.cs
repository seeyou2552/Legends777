using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject make_monster;
    [SerializeField] private int stage = 1;
    void Start()
    {
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        for (int i = 0; i < stage + 4; i++)
        {
            Vector3 pos = make_monster.transform.position;
            pos.x = Random.Range(-10,11);
            pos.y = Random.Range(-5,6);
            make_monster.transform.position = pos;
            Instantiate(make_monster);
        }
    }

}
