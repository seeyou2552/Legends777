using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerManager : Player
{
    Animator animation;
    SpriteRenderer renderer;

    [Header("체력설정")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;


    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDie;

    void Awake()
    {
        animation = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        Debug.Log("PlayerManager Awake");
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    void OnTriggerStay2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("Monster"))
        {
            // 데미지 (Hit)
            OnDamage(other);
        }

    }

    void OnDamage(Collider2D other)
    {
        Debug.Log("hit");
        animation.SetBool("IsHit", true);

        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");
        renderer.color = new Color(1, 1, 1, 0.4f);
        StartCoroutine(OnInvincibility());

        var stat = other.GetComponent<MonsterStatHandler>();
        int dmg = stat != null ? stat.Atk : 0;

        currentHealth = Mathf.Max(currentHealth - dmg, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if(currentHealth <= 0)
        {
            OnPlayerDie?.Invoke();
        }
    }

    IEnumerator OnInvincibility()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        animation.SetBool("IsHit", false);
        renderer.color = new Color(1, 1, 1, 1);
    }
}
