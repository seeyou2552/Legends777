using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    // ë°©í–¥ ?¤ì • ?¨ìˆ˜
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // ë§??„ë ˆ???´ë™ ì²˜ë¦¬
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // ?”ë©´ ë°–ìœ¼ë¡??˜ê?ë©??€ë¡?ë°˜í™˜
    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    // ì¶©ëŒ ??ì²˜ë¦¬
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.ApplyDamage(5);
            Debug.Log("Player HP: " + PlayerController.Instance.hp);
            if(PlayerController.Instance.hp <= 0)
            {
                Destroy(BossManager.instance.PlayerTarget);
            }
            ReturnToPool();
        }
        // ëª¬ìŠ¤?°ë‚˜ ?¤ë¥¸ ?Œì´?´ë³¼ê³?ì¶©ëŒ?˜ì? ?Šì•˜???Œì—ë§?ë°˜í™˜
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            ReturnToPool();
        }
    }

    // ?¤ë¸Œ?íŠ¸ ?€ë¡?ë°˜í™˜?˜ëŠ” ?¨ìˆ˜
    private void ReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}