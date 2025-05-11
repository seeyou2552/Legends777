using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public void teleport()
    {
        PlayerController.Instance.rigid.position = BossManager.instance.Rigid.position;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            teleport();
        }
    }
}
