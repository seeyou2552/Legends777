using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeWeaponHandler : MonsterWeaponHandler
{
    public Vector2 colliderBoxSize = Vector2.one;   //공격 범위 (충돌 박스 크기)
    public LayerMask target;    //공격 가능한 대상 레이어

    private void Start()
    {
        colliderBoxSize = colliderBoxSize * WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();

        Vector2 boxCenter = (Vector2)transform.position + Controller.LookDirection * colliderBoxSize.x;
        Collider2D hit = Physics2D.OverlapBox(boxCenter, colliderBoxSize, 0, target);


        //타겟과 충돌
        if (hit != null)
        {
            hit.GetComponent<Player>().hp -= Controller.gameObject.GetComponent<MonsterStatHandler>().Atk;
            Debug.Log(hit.GetComponent<Player>().hp);
        }
    }

    public override void Rotate(bool isLeft)
    {
        if (isLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        // LookDirection이 null이거나 크기 설정이 안됐을 때를 방지
        if (colliderBoxSize == Vector2.zero)
            return;

        // 플레이어 앞쪽에 위치한 박스 중심 계산
        Vector2 boxCenter = (Vector2)transform.position + Controller.LookDirection * colliderBoxSize.x;

        // Gizmo 색 설정
        Gizmos.color = Color.red;

        // OverlapBox와 동일한 위치, 크기로 사각형 그리기
        Gizmos.DrawWireCube(boxCenter, colliderBoxSize);
    }
}
