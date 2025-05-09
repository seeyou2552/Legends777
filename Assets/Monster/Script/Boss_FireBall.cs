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

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject); // ȭ�� ������ ������ ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
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
