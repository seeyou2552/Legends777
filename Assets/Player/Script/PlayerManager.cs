using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    Animator animator;
    SpriteRenderer renderer;
    Rigidbody2D rigid;

    [Header("臾댁쟻")]
    [SerializeField] float invincibilityDuration = 0.2f;
    [SerializeField] private AudioClip attackedSound;

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

        // Player.cs ???좎뼵??珥덇린 hp 瑜??쎌뼱??
    }

    private void Start()
    {
        maxHealth = PlayerController.Instance.hp;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (invincibilityTimer > 0f)
            invincibilityTimer -= Time.deltaTime;
    }

    // <-- ?ш린??紐⑤뱺 異⑸룎??泥섎━?⑸땲??
    void OnCollisionEnter2D(Collision2D col)
    {
        if (invincibilityTimer > 0f) return;

        var go = col.gameObject;
        if (go.CompareTag("Boss"))
        {
            // (?덉떆) 蹂댁뒪 紐명넻 ?덊듃 ?곕?吏
            ApplyDamage(10);
        }
        else if (go.CompareTag("Monster"))
        {
            var ms = go.GetComponent<MonsterStatHandler>();
            if (ms != null) ApplyDamage(ms.Atk);
        }
        //else if (go.CompareTag("FireBall"))
        //{
        //    // 蹂댁뒪 FireBall
        //    ApplyDamage(10);
        //    Destroy(go);
        //}
    }

    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if (invincibilityTimer > 0f) return;

        var go = col.gameObject;
        //if (go.CompareTag("Projectile"))
        //{
        //    var proj = go.GetComponent<ProjectileController>();
        //    if (proj?.monsterController != null)
        //    {
        //        var ms = proj.monsterController.GetComponent<MonsterStatHandler>();
        //        if (ms != null) ApplyDamage(ms.Atk);
        //    }
        //    Destroy(go);
        //}
    }
    */

    public void ApplyDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        PlayerController.Instance.hp = currentHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        animator.SetBool("IsHit", true);
        renderer.color = new Color(1f, 1f, 1f, 0.4f);
        invincibilityTimer = invincibilityDuration;
        StartCoroutine(EndInvincibility());

        if (currentHealth <= 0)
            OnPlayerDie?.Invoke();

        SoundManager.Instance.PlaySFX(attackedSound);
    }

    IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        animator.SetBool("IsHit", false);
        renderer.color = Color.white;
    }
}
