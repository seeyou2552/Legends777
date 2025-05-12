using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    float timer = 0;
    public float destroyTime = 3f;
    public GameObject swordPrefab;
    public GameObject arrowPrefab;
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
        if (timer >= destroyTime)
        {
            Destroy(this.gameObject);
        }

        if (skill.addBurn) BurnArrow();

        if (skill.addBomb > 0 && this.gameObject.name == "Weapon_Bomb(Clone)") StartCoroutine(Spread());

        if (skill.addSpread > 0 && this.gameObject.name == "Weapon_Arrow(Clone)") StartCoroutine(Spread());

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("ì ì¤‘");
            // addBurn íš¨ê³¼
            if (skill.addBurn)
            {
                SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
                otherRenderer.color = new Color(1f, 0f, 0f, 0.4f);
            }

            if (!skill.addPenetrates) Destroy(this.gameObject); // ë¹„ê´€í†µ
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    void BurnArrow()
    {
        renderer.color = new Color(1f, 0f, 0f, 0.5f);
    }

    IEnumerator Spread()
    {
        yield return new WaitForSeconds(1f);
        GameObject weapon;
        int count;
        if(this.gameObject.name == "Weapon_Bomb(Clone)") count = skill.addBomb + 2;
        else count = skill.addSpread + 2;

        float angleStep = 360f / count;


        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            if (this.gameObject.name == "Weapon_Arrow(Clone)")
            {
                weapon = Instantiate(swordPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                weapon = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            }

            Rigidbody2D rigid = weapon.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * skill.shootSpeed;

            float zRot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, zRot - 90f);
        }
        Destroy(this.gameObject);

    }
}
