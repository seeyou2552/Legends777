using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    // 방향 설정 함수
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // 매 프레임 이동 처리
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // 화면 밖으로 나가면 풀로 반환
    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    // 충돌 시 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.ApplyDamage(5);
            Debug.Log("Player HP: " + Player.Instance.hp);
            if(Player.Instance.hp <= 0)
            {
                Destroy(BossManager.instance.PlayerTarget);
            }
            ReturnToPool();
        }
        // 몬스터나 다른 파이어볼과 충돌하지 않았을 때에만 반환
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            ReturnToPool();
        }
    }

    // 오브젝트 풀로 반환하는 함수
    private void ReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}