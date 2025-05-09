using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeWeaponHandler : MonsterWeaponHandler
{
    public Vector2 colliderBoxSize = Vector2.one;   //���� ���� (�浹 �ڽ� ũ��)
    public LayerMask target;    //���� ������ ��� ���̾�

    private void Start()
    {
        colliderBoxSize = colliderBoxSize * WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();

        Vector2 boxCenter = (Vector2)transform.position + Controller.LookDirection * colliderBoxSize.x;
        Collider2D hit = Physics2D.OverlapBox(boxCenter, colliderBoxSize, 0, target);


        //Ÿ�ٰ� �浹
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
        // LookDirection�� null�̰ų� ũ�� ������ �ȵ��� ���� ����
        if (colliderBoxSize == Vector2.zero)
            return;

        // �÷��̾� ���ʿ� ��ġ�� �ڽ� �߽� ���
        Vector2 boxCenter = (Vector2)transform.position + Controller.LookDirection * colliderBoxSize.x;

        // Gizmo �� ����
        Gizmos.color = Color.red;

        // OverlapBox�� ������ ��ġ, ũ��� �簢�� �׸���
        Gizmos.DrawWireCube(boxCenter, colliderBoxSize);
    }
}
