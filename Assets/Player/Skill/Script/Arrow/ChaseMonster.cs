using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseMonster : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D arrowRigid;
    public CircleCollider2D collider;
    SkillManager skill;

    void Start()
    {
        GameObject bow = GameObject.Find("Weapon_Bow");
        skill = bow.GetComponent<SkillManager>();
        arrowRigid = transform.parent.GetComponent<Rigidbody2D>();
        collider = this.gameObject.GetComponent<CircleCollider2D>();
        collider.radius = skill.chaseRadius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (skill.addChase == false) return;
        if (skill.addChase && other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Boss"))
        {
            target = other.gameObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (target != null && arrowRigid != null)
        {
            Vector3 direction3D = (target.position - arrowRigid.transform.position).normalized;

            // 화살이 목표를 향하도록 회전 (z축을 포함하여 회전)
            float angle = Mathf.Atan2(direction3D.y, direction3D.x) * Mathf.Rad2Deg;

            // 화살의 회전 값 설정
            arrowRigid.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

            // 속도 설정
            arrowRigid.velocity = direction3D * skill.shootSpeed;
        }
    }
}
