using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    Animator animator;
    SpriteRenderer renderer;
    Rigidbody2D rigid;

    [Header("무적")]
    [SerializeField] float invincibilityDuration = 0.2f;
    float invincibilityTimer;

    int maxHealth, currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDie;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        // Player.cs 에 선언된 초기 hp 를 읽어서
        maxHealth = Player.Instance.hp;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (invincibilityTimer > 0f)
            invincibilityTimer -= Time.deltaTime;
    }

    // <-- 여기서 모든 충돌을 처리합니다
    void OnCollisionEnter2D(Collision2D col)
    {
        if (invincibilityTimer > 0f) return;

        var go = col.gameObject;
        if (go.CompareTag("Boss"))
        {
            // (예시) 보스 몸통 히트 데미지
            ApplyDamage(10);
        }
        else if (go.CompareTag("Monster"))
        {
            var ms = go.GetComponent<MonsterStatHandler>();
            if (ms != null) ApplyDamage(ms.Atk);
        }
        //else if (go.CompareTag("FireBall"))
        //{
        //    // 보스 FireBall
        //    ApplyDamage(10);
        //    Destroy(go);
        //}
    }

    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if (invincibilityTimer > 0f) return;

        var go = col.gameObject;
        if (go.CompareTag("Projectile"))
        {
            var proj = go.GetComponent<ProjectileController>();
            if (proj?.monsterController != null)
            {
                var ms = proj.monsterController.GetComponent<MonsterStatHandler>();
                if (ms != null) ApplyDamage(ms.Atk);
            }
            Destroy(go);
        }
    }
    */

    public void ApplyDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Player.Instance.hp = currentHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        animator.SetBool("IsHit", true);
        renderer.color = new Color(1f, 1f, 1f, 0.4f);
        invincibilityTimer = invincibilityDuration;
        StartCoroutine(EndInvincibility());

        if (currentHealth <= 0)
            OnPlayerDie?.Invoke();
    }

    IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        animator.SetBool("IsHit", false);
        renderer.color = Color.white;
    }
}
