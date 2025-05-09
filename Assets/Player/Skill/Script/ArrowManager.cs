using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    float timer;
    public float destroyTime;
    SkillManager skill;

    SpriteRenderer renderer;

    void Awake()
    {
        GameObject bow = GameObject.Find("Weapon_Bow");
        skill = bow.GetComponent<SkillManager>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= destroyTime)
        {
            Destroy(this.gameObject);
        }

        if(skill.addBurn) BurnArrow();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MonsterController monster = other.GetComponent<MonsterController>();
        if (monster != null)
        {
            Debug.Log("monster attack");
            monster.OnDamaged(Player.Instance.power);
            // addBurn ?¨ê³¼
            if(skill.addBurn)
            {
                SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
                otherRenderer.color = new Color(1f, 0f, 0f, 0.4f);
            }
            
            if(!skill.addPenetrates) 
                Destroy(this.gameObject); // ë¹„ê???
        }

        if(other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    void BurnArrow()
    {
        renderer.color = new Color(1f, 0f, 0f, 0.5f);
    }
}
