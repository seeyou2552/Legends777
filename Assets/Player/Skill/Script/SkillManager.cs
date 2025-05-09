using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class SkillManager : Skill
{
    public GameObject arrowPrefab;
    public GameObject bowPrefab;
    public GameObject target;
    public float shootInterval = 2f;
    public float chaseRadius = 0f;
    public float timer;
    public bool createBow = false;

    void Awake()
    {
        target = GameObject.Find("Player");
    }
    void Update()
    {
        timer += Time.deltaTime;
        AutoAttack();
    }

    void AutoAttack()
    {
        Debug.Log(timer);
        if (timer >= shootInterval)
        {
            if (createBow)
            {
                ShootArrow();
            }
            else if (addGhost) GhostShoot();
            else ShootArrow();

            // if(addSword)

            timer = 0f;
        }
    }

    void ShootArrow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 발사체 생성
        Vector2 direction = (new Vector2(mousePos.x, mousePos.y) - (Vector2)transform.position).normalized;

        float fullAngle = (arrowCount > 1) ? Mathf.Clamp(15f * (arrowCount - 1), 0f, 90f) : 0f;
        float startAngle = -fullAngle / 2f;

        for (int i = 0; i < arrowCount; i++)
        {
            // 퍼지는 각도 계산
            float angleOffset = (arrowCount > 1) ? startAngle + (fullAngle / (arrowCount - 1)) * i : 0f;

            // 회전된 방향 벡터 계산
            float angleInRad = Mathf.Atan2(direction.y, direction.x) + angleOffset * Mathf.Deg2Rad;
            Vector2 rotatedDirection = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad)).normalized;

            // Z 회전값 계산
            float zRotation = Mathf.Atan2(rotatedDirection.y, rotatedDirection.x) * Mathf.Rad2Deg - 90f;

            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, zRotation));

            SpriteRenderer renderer = arrow.GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                renderer.flipX = rotatedDirection.x < 0;
            }

            Rigidbody2D rigid = arrow.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.velocity = rotatedDirection * shootSpeed;
            }
        }
    }

    void GhostShoot()
    {
        createBow = true;
        GameObject bowInstance = Instantiate(bowPrefab, new Vector3(target.transform.position.x + 2f, target.transform.position.y, 0f), Quaternion.identity);
        StartCoroutine(GhostBow(bowInstance));
    }

    IEnumerator GhostBow(GameObject bow)
    {
        ShootArrow();
        addGhost = false;
        yield return new WaitForSeconds(shootInterval);
        Destroy(bow);
        addGhost = true;
        createBow = false;
    }
    

}
