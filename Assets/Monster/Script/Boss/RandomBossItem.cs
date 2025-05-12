using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBossItem : MonoBehaviour
{
    [SerializeField] private Sprite[] itemSprite;
    
    private void Start()
    {
        int Srandom = UnityEngine.Random.Range(0, 4);

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.sprite = itemSprite[Srandom];

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                BossSkillManager.Instance.StartCoroutine(BossSkillManager.Instance.MakeRandomEffect());
            }
            else
            {
                BossSkillManager.Instance.StartCoroutine(BossSkillManager.Instance.MakeRandomBedEffect());
            }
            BossObjectPoolManager.Instance.ReturnToPool("item", this.gameObject);
        }
    }
    
}
