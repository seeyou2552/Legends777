using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    // 방향 ?�정 ?�수
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // �??�레???�동 처리
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // 충돌 ??처리
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
        // 몬스?�나 ?�른 ?�이?�볼�?충돌?��? ?�았???�에�?반환
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            bReturnToPool();
        }
    }

    // ?�브?�트 ?��?반환?�는 ?�수
    private void bReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}