using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class ArrowManager : MonoBehaviour
{
    float timer = 0;
    public float destroyTime = 3f;
    public GameObject swordPrefab;
    public GameObject arrowPrefab;
    SkillManager skill;
    Player player;
    SpriteRenderer renderer;
    private AudioSource audio;

    void Awake()
    {
        GameObject bow = GameObject.Find("Weapon_Bow");
        skill = bow.GetComponent<SkillManager>();
        GameObject target = GameObject.Find("Player");
        player = target.GetComponent<Player>();
        renderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTime)
        {
            Destroy(this.gameObject);
        }

        if (skill.addFreeze) FreezeArrow();

        if (skill.addBomb <= 3 && skill.addBomb > 0 && this.gameObject.name == "Weapon_Bomb(Clone)") StartCoroutine(Spread());

        if (skill.addSpread <= 3 && skill.addSpread > 0 && this.gameObject.name == "Weapon_Arrow(Clone)") StartCoroutine(Spread());

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터 적중
        if (other.gameObject.CompareTag("Monster"))
        {
            MonsterController monster = other.GetComponent<MonsterController>();
            monster.OnDamaged(Player.Instance.power);

            if (skill.addFreeze)
            {
                MonsterStatHandler monsterStat = other.GetComponent<MonsterStatHandler>();
                if (monsterStat.Speed > 0) monsterStat.Speed -= 1;
            }

            if (skill.addPenetrates)
            {
                if(skill.addChase) Destroy(this.gameObject);
            }
            else Destroy(this.gameObject); // ë¹„ê´€í†µ
        }

        // 보스 적중
        if (other.gameObject.CompareTag("Boss"))
        {
            if (!skill.addPenetrates) Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    void FreezeArrow()

    {
        renderer.color = new Color(0.4f, 0.7f, 1f, 1f);
    }

    IEnumerator Spread()
    {
        yield return new WaitForSeconds(1f);
        GameObject weapon;
        int count;
        if (this.gameObject.name == "Weapon_Bomb(Clone)") count = skill.addBomb + 2;
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
