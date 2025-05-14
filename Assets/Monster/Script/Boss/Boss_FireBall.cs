using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.ApplyDamage(5);
            if(PlayerController.Instance.hp <= 0) // 플레이어가 죽으면 보스 파괴 및 파이어볼 반환
            {
                Destroy(BossManager.instance.bossObject);
            }
            bReturnToPool();
        }
        // 몬스터나 파이어 볼이 아니면 반환
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            bReturnToPool();
        }
    }

    // 반환 함수
    private void bReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}