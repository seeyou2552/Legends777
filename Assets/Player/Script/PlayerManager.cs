using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[SerializeField]
public class PlayerManager : Player
{
    Animator animation;
    SpriteRenderer renderer;

    [Header("무적(Invincibility) 설정")]
    [SerializeField] private float invincibilityDuration = 0.2f;
    private float invincibilityTimer = 0f;


    private int maxHealth;
    private int currentHealth;
    private int prevRawHp;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDie;

    void Awake()
    {
        animation = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        Debug.Log("PlayerManager Awake");
        maxHealth = Player.Instance.hp;
        currentHealth = maxHealth;
        prevRawHp = Player.Instance.hp;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

    }

    private void Update()
    {
        if (invincibilityTimer > 0f)
            invincibilityTimer -= Time.deltaTime;
        int rawHp = Player.Instance.hp;
        if (rawHp < prevRawHp && invincibilityTimer <= 0f)
        {
            int dmg = prevRawHp - rawHp;
            OnDamage(dmg);

            prevRawHp = Player.Instance.hp;
        }
        else
        {
            prevRawHp = rawHp;
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invincibilityTimer > 0f)
            return;
        // A) 몬스터/보스 몸통 태그가 붙어 있으면
        if (other.tag == "Monster" || other.tag == "Boss")
        {
            int dmg = 0;
            var ms = other.GetComponent<MonsterStatHandler>();
            if (ms != null) dmg = ms.Atk;
            OnDamage(dmg);
            return;
        }

        var proj = other.GetComponent<ProjectileController>();
        if (proj != null)
        {
            // proj.monsterController가 null인지 체크
            var ms = proj.monsterController?.GetComponent<MonsterStatHandler>();
            int dmg = (ms != null) ? ms.Atk : 0;
            OnDamage(dmg);

            Destroy(other.gameObject);
            return;
        }

    }

    public void OnDamage(int damage)
    {
        Debug.Log("hit");
        animation.SetBool("IsHit", true);

        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");
        renderer.color = new Color(1, 1, 1, 0.4f);
        invincibilityTimer = invincibilityDuration;
        StartCoroutine(OnInvincibility());

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Player.Instance.hp = currentHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        

        if (currentHealth <= 0)
        {
            OnPlayerDie?.Invoke();
        }
    }

    IEnumerator OnInvincibility()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        gameObject.layer = LayerMask.NameToLayer("Player");
        animation.SetBool("IsHit", false);
        renderer.color = new Color(1, 1, 1, 1);
    }
}
