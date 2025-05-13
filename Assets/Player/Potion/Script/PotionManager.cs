using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : Potion
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player.Instance.hp += hp;
            if(Player.Instance.hp >= 100) Player.Instance.hp = 100;

            Player.Instance.attackSpeed -= attackSpeed;

            Player.Instance.power += power;

            Destroy(this.gameObject);
        }
    }
}
