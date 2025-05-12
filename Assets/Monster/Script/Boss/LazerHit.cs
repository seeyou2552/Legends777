using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.ApplyDamage(20);
            Debug.Log("hit"); // µ¥¹ÌÁö Ã³¸® º¯°æ ¿¹Á¤
        }
    }
}