using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float monsterSpeed = 3; // 몬스터 속도
    public Rigidbody2D target; // 타겟 확인

    private bool isAlive; // 살았는지 죽었는지 체크

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = target.position - rigid.position; // 타겟과 몬스터의 위치 길이 구하기
        Vector2 nextDir = direction.normalized * monsterSpeed * Time.fixedDeltaTime; // 플레이어에게 향하는 움직임
        rigid.MovePosition(rigid.position + nextDir); // 움직임 구현
        rigid.velocity = Vector2.zero; //물리 속도가 이동에 영향을 주지 않도록 제거
    }

    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // 몬스터가 플레이어보다 x위치가 다르면 방향 변경
    }
}
