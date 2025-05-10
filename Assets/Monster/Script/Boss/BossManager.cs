using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int bossHealth = 10;
    public int Health
    {
        get => bossHealth;
        set => bossHealth = Mathf.Clamp(value, 0, 1000);
    }

    [Range(1f, 20f)][SerializeField] private float bossSpeed = 3f;
    public float MonsterSpeed
    {
        get => bossSpeed;
        set => bossSpeed = Mathf.Clamp(value, 0, 20);
    }

    private Rigidbody2D target; // Ÿ�� Ȯ��

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    public static BossManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    

    private void FixedUpdate()
    {
        Vector2 direction = target.position - rigid.position; // Ÿ�ٰ� ������ ��ġ ���� ���ϱ�

        float stopDistance = 1.5f; // Ÿ�ٰ��� �ּ� �Ÿ�

        if (direction.magnitude >= stopDistance)
        {
            Vector2 nextDir = direction.normalized * bossSpeed * Time.fixedDeltaTime; // �÷��̾�� ���ϴ� ������
            rigid.MovePosition(rigid.position + nextDir); // ������ ����
            animator.SetBool("isRun", true);
        }
        else
        {
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
