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

    private Rigidbody2D target; // 타겟 확인

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
        Vector2 direction = target.position - rigid.position; // 타겟과 몬스터의 위치 길이 구하기

        float stopDistance = 1.5f; // 타겟과의 최소 거리

        if (direction.magnitude >= stopDistance)
        {
            Vector2 nextDir = direction.normalized * bossSpeed * Time.fixedDeltaTime; // 플레이어에게 향하는 움직임
            rigid.MovePosition(rigid.position + nextDir); // 움직임 구현
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // 몬스터가 플레이어보다 x위치가 다르면 방향 변경
    }

    private void OnEnable()
    {
        target = PlayerController.Instance.GetComponent<Rigidbody2D>();
    }

    
}
