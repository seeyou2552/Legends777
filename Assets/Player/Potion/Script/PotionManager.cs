using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : Potion
{
    private UI_GameScene ui;
    private PlayerController player;
    public int tempHp;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        ui = FindObjectOfType<UI_GameScene>();
        tempHp = player.hp;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name.StartsWith("HP_Potion"))
            {
                PlayerController.Instance.hp += hp;
                if (PlayerController.Instance.hp >= tempHp) PlayerController.Instance.hp = tempHp;
                ui.SetStatus(this.gameObject.name, hp);
            }

            if (this.gameObject.name.StartsWith("AttackSpeed_Potion"))
            {
                PlayerController.Instance.attackSpeed -= attackSpeed;
                ui.SetStatus(this.gameObject.name, attackSpeed);
            }

            if (this.gameObject.name.StartsWith("Power_Potion"))
            {
                PlayerController.Instance.power += power;
                ui.SetStatus(this.gameObject.name, power);
            }

            Destroy(this.gameObject);
        }
    }
}
