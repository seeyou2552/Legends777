using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject weaponprefabs;
    public Rigidbody2D target; // Ÿ�� Ȯ��

    [SerializeField] private float monsterSpeed = 3; // ���� �ӵ�

    private bool isAlive; // ��Ҵ��� �׾����� üũ

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (spriter.name.Contains("Far")) // ���Ÿ��� �������� �ʰ�
        {
            rigid.velocity = Vector2.zero;
            return;
        }
        Vector2 direction = target.position - rigid.position; // Ÿ�ٰ� ������ ��ġ ���� ���ϱ�
        
        float stopDistance = 1.5f; // Ÿ�ٰ��� �ּ� �Ÿ�

        if (direction.magnitude > stopDistance)
        {
            Vector2 nextDir = direction.normalized * monsterSpeed * Time.fixedDeltaTime; // �÷��̾�� ���ϴ� ������
            rigid.MovePosition(rigid.position + nextDir); // ������ ����
            animator.SetBool("isRun", true);
        }
        else
        {
            rigid.velocity = Vector2.zero; // ���������Ƿ� ���߱�
            animator.SetBool("isRun", false);
        }
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // ���Ͱ� �÷��̾�� x��ġ�� �ٸ��� ���� ����
    }

    private void OnEnable()
    {
        target = PlayerController.Instance.GetComponent<Rigidbody2D>();
    }
}
