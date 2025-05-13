using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : Potion
{
    private PlayerController player;
    public int tempHp;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        tempHp = player.hp;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.hp += hp;
            if (PlayerController.Instance.hp >= tempHp) PlayerController.Instance.hp = tempHp;

            PlayerController.Instance.attackSpeed -= attackSpeed;

            PlayerController.Instance.power += power;

            Destroy(this.gameObject);
        }
    }
}
