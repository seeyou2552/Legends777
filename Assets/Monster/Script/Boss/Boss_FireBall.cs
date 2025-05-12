using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    //�⺻ ����
    // ������ �⺻������ ���̾�� �ϳ��� 2�������� 3��°�� ������� ���̾ �߻� ��ƾ ���̾� ���� �ѹ� ���� ƨ��� 2��° ���� ������ �ı�
    // ��ų ������ ������ 2�� ���� ������x
    // ���� ���� �浹�� ������ // �̱���
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
            Player.Instance.hp -= 10;
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


    // ��ų �� ������ ��� ���� ��ų
    // ��ų ������ 5�ʸ��� �ѹ��� ����
    // �÷��̾�� ������ ���� 1 ~ ��
    // ����� ��ȯ 3 ��

    // ���̾ �������� 5�ʰ� ���� �÷��̾ �����ϸ鼭 4 ~ ��
    // ������(����) ���ڰ� �߻� �� 1�� �� x�ڷ� �߻� �̰� ���� ���� 6 ��

    // ��ü�� �ڽ��� �������� ������ �����ٰ� �ѹ��� �߻� 7 ~
    // ī�޶� ���� ȿ�� 10�ʰ� ���� 9 ��

    // ȭ���� ���������� �ٲ㼭 ���� �� ������ ������ �ʰ� 10�ʰ� ���� 10 ~ ��
    // ���� �����ϸ� �� ���� �ϋ� �п� �ǰ� 50%�� �Ѵ� �� 50%�� �������·� �п� 
}
