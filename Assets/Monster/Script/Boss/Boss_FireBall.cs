using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    // Î∞©Ìñ• ?§Ï†ï ?®Ïàò
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // Îß??ÑÎ†à???¥Îèô Ï≤òÎ¶¨
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // Ï∂©Îèå ??Ï≤òÎ¶¨
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
            bReturnToPool();
        }
        // Î™¨Ïä§?∞ÎÇò ?§Î•∏ ?åÏù¥?¥Î≥ºÍ≥?Ï∂©Îèå?òÏ? ?äÏïò???åÏóêÎß?Î∞òÌôò
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            bReturnToPool();
        }
    }

    // ?§Î∏å?ùÌä∏ ?ÄÎ°?Î∞òÌôò?òÎäî ?®Ïàò
    private void bReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}