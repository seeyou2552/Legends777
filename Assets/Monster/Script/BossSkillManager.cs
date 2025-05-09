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

    private List<System.Func<IEnumerator>> skillFuncs = new List<System.Func<IEnumerator>>(); // 코루틴을 담기 위한 리스트
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
        Vector2 spawnPos = firePoint.position + (Vector3)(direction * 0.5f); // 거리 조절
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        Collider2D bossCol = GetComponent<Collider2D>(); // 각각의 공격 과 보스는 무시
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
        yield return new WaitForSeconds(5f); // 5초 대기
        while (true)
        {
            if (skillFuncs.Count > 0)
            {
                yield return StartCoroutine(skillFuncs[currentSkillIndex]());
                int num = Random.Range(0, skillFuncs.Count);
                currentSkillIndex = (currentSkillIndex + num) % skillFuncs.Count; // 다음 스킬 인덱스 계산
            }

            yield return new WaitForSeconds(2f); // 5초 대기
        }
    }

    private IEnumerator MoveFast()//플레이어에게 빠르게 돌진
    {
        BossManager.Instance.MonsterSpeed = 8;
        yield return new WaitForSeconds(5f);
        BossManager.Instance.MonsterSpeed = 3;
    }

    private IEnumerator MakeMonster()// 잡몹들 소환
    {
        MonsterManager.Instance.SpawnRandomMonster();
        yield return null;
    }

    private IEnumerator CameraReversal() // 카메라 반전
    {
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return new WaitForSeconds(10f);
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private IEnumerator ShootFast()
    {
        fireRate = 15;
        currentBulletSpeed = 25f;  // 총알 속도 빠르게
        yield return new WaitForSeconds(5f);
        currentBulletSpeed = defaultBulletSpeed; // 다시 기본 속도
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
