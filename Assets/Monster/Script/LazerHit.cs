using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit"); // ������ ó�� ���� ����
        }
    }
}
