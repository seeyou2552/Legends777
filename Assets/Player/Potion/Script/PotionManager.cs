using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : Potion
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.hp += hp;
            if(PlayerController.Instance.hp >= 100) PlayerController.Instance.hp = 100;

            PlayerController.Instance.attackSpeed -= attackSpeed;

            PlayerController.Instance.power += power;

            Destroy(this.gameObject);
        }
    }
}
