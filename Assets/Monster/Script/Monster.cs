using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float monsterSpeed = 3; // ���� �ӵ�
    public Rigidbody2D target; // Ÿ�� Ȯ��

    private bool isAlive; // ��Ҵ��� �׾����� üũ

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = target.position - rigid.position; // Ÿ�ٰ� ������ ��ġ ���� ���ϱ�
        Vector2 nextDir = direction.normalized * monsterSpeed * Time.fixedDeltaTime; // �÷��̾�� ���ϴ� ������
        rigid.MovePosition(rigid.position + nextDir); // ������ ����
        rigid.velocity = Vector2.zero; //���� �ӵ��� �̵��� ������ ���� �ʵ��� ����
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // ���Ͱ� �÷��̾�� x��ġ�� �ٸ��� ���� ����
    }
}
