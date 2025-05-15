using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI_BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;

    private BossManager boss;

    public void Init(BossManager boss)
    {
        this.boss = boss;
        slider.maxValue = boss.MaxHealth;
        slider.value = boss.Health;
        healthText.text = $"{boss.Health}/{boss.MaxHealth}";

        boss.OnHealthChanged += OnHealthChanged;
        boss.Ondead += OnDead;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        slider.value = currentHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }
}
