using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject weaponprefabs;
    public Rigidbody2D target; // 타겟 확인

    [SerializeField] private float monsterSpeed = 3; // 몬스터 속도

    private bool isAlive; // 살았는지 죽었는지 체크

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
        if (spriter.name.Contains("Far")) // 원거리는 움직이지 않게
        {
            rigid.velocity = Vector2.zero;
            return;
        }
        Vector2 direction = target.position - rigid.position; // 타겟과 몬스터의 위치 길이 구하기
        
        float stopDistance = 1.5f; // 타겟과의 최소 거리

        if (direction.magnitude > stopDistance)
        {
            Vector2 nextDir = direction.normalized * monsterSpeed * Time.fixedDeltaTime; // 플레이어에게 향하는 움직임
            rigid.MovePosition(rigid.position + nextDir); // 움직임 구현
            animator.SetBool("isRun", true);
        }
        else
        {
            rigid.velocity = Vector2.zero; // 도착했으므로 멈추기
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
