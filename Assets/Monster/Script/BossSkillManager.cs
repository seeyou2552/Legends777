using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject redGround;
    public GameObject RedLazer1;
    public GameObject RedLazer2;
    private Camera Boss_Camera;
    public Transform firePoint;
    public Transform target;
    public float fireRate = 5f;
    public float defaultBulletSpeed = 15f;

    private float currentBulletSpeed;
    private float nextFireTime = 0f;
    private int attackCount = 1;

    private List<System.Func<IEnumerator>> skillFuncs = new List<System.Func<IEnumerator>>(); // �ڷ�ƾ�� ��� ���� ����Ʈ
    private int currentSkillIndex = 0;

    private void Start()
    {
        Boss_Camera = Camera.main;
        currentBulletSpeed = defaultBulletSpeed;
        skillFuncs.Add(MoveFast);
        skillFuncs.Add(MakeMonster);
        skillFuncs.Add(CameraReversal);
        skillFuncs.Add(ShootFast);
        skillFuncs.Add(RedGround);
        skillFuncs.Add(LazerPattern);
        StartCoroutine(UseSkillsRoutine());
    }
    

    void Update()
    {
        if (target != null && Time.time >= nextFireTime)
        {
            MakeFireBall();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void ShootFireball(Vector2 direction)
    {
        Vector2 spawnPos = firePoint.position + (Vector3)(direction * 0.5f); // �Ÿ� ����
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        Collider2D bossCol = GetComponent<Collider2D>(); // ������ ���� �� ������ ����
        Collider2D fireballCol = fireball.GetComponent<Collider2D>();
        if (fireballCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(fireballCol, bossCol);
        }

        Boss_FireBall fireballScript = fireball.GetComponent<Boss_FireBall>();
        fireballScript.SetDirection(direction.normalized);
        fireballScript.speed = currentBulletSpeed;
    }

    void MakeFireBall()
    {
        if (attackCount % 3 != 0) 
        {
            Vector2 direction = (target.position - firePoint.position).normalized;
            ShootFireball(direction);
            attackCount++;
        }
        else
        {
            attackCount = 1;
            int bulletCount = 8;
            float angleStep = 360f / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                ShootFireball(direction);
            }
        }
    }

    private IEnumerator UseSkillsRoutine()
    {
        yield return new WaitForSeconds(5f); // 5�� ���
        while (true)
        {
            if (skillFuncs.Count > 0)
            {
                yield return StartCoroutine(skillFuncs[currentSkillIndex]());
                int num = Random.Range(0, skillFuncs.Count);
                currentSkillIndex = (currentSkillIndex + num) % skillFuncs.Count; // ���� ��ų �ε��� ���
            }

            yield return new WaitForSeconds(2f); // 5�� ���
        }
    }

    private IEnumerator MoveFast()//�÷��̾�� ������ ����
    {
        BossManager.Instance.MonsterSpeed = 8;
        yield return new WaitForSeconds(5f);
        BossManager.Instance.MonsterSpeed = 3;
    }

    private IEnumerator MakeMonster()// ����� ��ȯ
    {
        MonsterManager.Instance.SpawnRandomMonster();
        yield return null;
    }

    private IEnumerator CameraReversal() // ī�޶� ����
    {
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return new WaitForSeconds(10f);
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private IEnumerator ShootFast()
    {
        fireRate = 15;
        currentBulletSpeed = 25f;  // �Ѿ� �ӵ� ������
        yield return new WaitForSeconds(5f);
        currentBulletSpeed = defaultBulletSpeed; // �ٽ� �⺻ �ӵ�
        fireRate = 5;
    }

    private IEnumerator RedGround()
    {
        GameObject Red = Instantiate(redGround);
        yield return new WaitForSeconds(10f);
        Destroy(Red);
    }

    private IEnumerator LazerPattern()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject Lazer = Instantiate(RedLazer1);
            yield return new WaitForSeconds(0.1f);
            Destroy(Lazer);
            Lazer = Instantiate(RedLazer2);
            yield return new WaitForSeconds(0.1f);
            Destroy(Lazer);
        }
    }
}
