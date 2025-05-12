using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    public float speed = 15f;
    private Vector2 direction;

    // ���� ���� �Լ�
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // �� ������ �̵� ó��
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // ȭ�� ������ ������ Ǯ�� ��ȯ
    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    // �浹 �� ó��
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
        // ���ͳ� �ٸ� ���̾�� �浹���� �ʾ��� ������ ��ȯ
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            ReturnToPool();
        }
    }

    // ������Ʈ Ǯ�� ��ȯ�ϴ� �Լ�
    private void ReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }
}
