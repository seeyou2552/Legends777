using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.hp -= 20;
            Destroy(BossManager.instance.PlayerTarget);
            Debug.Log("hit"); // ������ ó�� ���� ����
        }
    }
}
