using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class BossSkillManager : MonoBehaviour
{
    public Transform firePoint;
    private Transform target;
    private Camera Boss_Camera;

    public float fireRate = 5f;
    public float defaultBulletSpeed = 15f;

    private float currentBulletSpeed;
    private float nextFireTime = 0f;
    private int attackCount = 1;

    private List<System.Func<IEnumerator>> skillFuncs = new List<System.Func<IEnumerator>>();
    private int currentSkillIndex = 0;

    private List<GameObject> activeSkillObjects = new List<GameObject>(); // ���� �����Ǵ� ������Ʈ�� ������ ������ �����ϱ� ���� �����صδ� ����Ʈ

    BossManager bossManager;
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
        bossManager = GetComponent<BossManager>();
        Boss_Camera = Camera.main;
        currentBulletSpeed = defaultBulletSpeed;


        gameManager.OnStageUpdated += SkillsForStage; // �������� �̺�Ʈ ���
        SkillsForStage();

        StartCoroutine(UseSkillsRoutine());
    }

    void Update()
    {
        if (target != null && Time.time >= nextFireTime)
        {
            MakeFireBall();
            nextFireTime = Time.time + 1f / fireRate;
        }
        //transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    private void SkillsForStage() // ����� ��ų����Լ�
    {
        int stage = gameManager.Stage;
        skillFuncs.Clear();

        // ����� ��ų�� ���
        if (gameManager.Stage < 5)
        {
            skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
        }
        else if (gameManager.Stage < 10)
        {
            skillFuncs.Add(CameraReversal);
            skillFuncs.Add(ShootFast);
        }
        else if (gameManager.Stage < 15)
        {
            skillFuncs.Add(LazerPattern);
            skillFuncs.Add(RedGround);
            //skillFuncs.Add(CirCleFireball);
        }
    }

    void ShootFireball(Vector2 direction)
    {
        Vector2 spawnPos = firePoint.position + (Vector3)(direction * 0.5f);

        // ������Ʈ Ǯ���� ���̾ ������
        GameObject fireball = BossObjectPoolManager.Instance.GetFromPool("Fireball", spawnPos, Quaternion.identity);
        if (fireball == null) // �ı��� ������Ʈ�� ���
        {
            return; // �ش� ������Ʈ�� �� �̻� ����� �� �����Ƿ� ����
        }

        fireball.transform.SetParent(this.transform);
        // �浹 ó�� - ������ ���̾�� �浹���� �ʵ��� ����
        Collider2D bossCol = GetComponent<Collider2D>();
        Collider2D fireballCol = fireball.GetComponent<Collider2D>();
        if (fireballCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(fireballCol, bossCol);
        }

        // ���̾�� ��ũ��Ʈ ���� �� ����, �ӵ� ����
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
            int bulletCount = 8; // �߻��� ���̾�� ����
            float angleStep = 360f / bulletCount; //�� 360���� bulletCount�� ����� 45�� �������� �߻�

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad; //���� ������ ��ȯ
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); //Cos�� x�� ����, Sin�� y�� ���� �̰ɷ� unit circle���� ������ ���
                ShootFireball(direction);
            }
        }
    }

    private IEnumerator UseSkillsRoutine()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            if (skillFuncs.Count > 0)
            {
                yield return StartCoroutine(skillFuncs[currentSkillIndex]());
                int num = Random.Range(0, skillFuncs.Count);
                currentSkillIndex = (currentSkillIndex + num) % skillFuncs.Count;
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator MoveFast()
    {
        bossManager.MonsterSpeed = 8;
        yield return new WaitForSeconds(5f);
        bossManager.MonsterSpeed = 3;
    }

    private IEnumerator MakeMonster()
    {
        MonsterManager.Instance.SpawnRandomMonster();
        yield return null;
    }

    private IEnumerator CameraReversal()
    {
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return new WaitForSeconds(5f);
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private IEnumerator ShootFast()
    {
        fireRate = 15;
        currentBulletSpeed = 25f;
        yield return new WaitForSeconds(5f);
        currentBulletSpeed = defaultBulletSpeed;
        fireRate = 5;
    }

    private IEnumerator RedGround()
    {
        GameObject red = BossObjectPoolManager.Instance.GetFromPool("redGround", Vector2.zero, Quaternion.identity);
        if (!activeSkillObjects.Contains(red))
        {
            activeSkillObjects.Add(red);
        }
        yield return new WaitForSeconds(5f);
        BossObjectPoolManager.Instance.ReturnToPool("redGround", red);
    }

    private IEnumerator LazerPattern()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject lazer1 = BossObjectPoolManager.Instance.GetFromPool("redLazer1", Vector2.zero, Quaternion.identity);
            if(!activeSkillObjects.Contains(lazer1))
            {
                activeSkillObjects.Add(lazer1);
            }
            yield return new WaitForSeconds(0.1f);
            BossObjectPoolManager.Instance.ReturnToPool("redLazer1", lazer1);

            GameObject lazer2 = BossObjectPoolManager.Instance.GetFromPool("redLazer2", Vector2.zero, Quaternion.identity);
            if (!activeSkillObjects.Contains(lazer2))
            {
                activeSkillObjects.Add(lazer2);
            }
            yield return new WaitForSeconds(0.1f);
            BossObjectPoolManager.Instance.ReturnToPool("redLazer2", lazer2);
        }
    }

    //private IEnumerator CirCleFireball()
    //{
    //    int bulletCount = 8; // �߻��� ���̾�� ����
    //    float angleStep = 360f / bulletCount; //�� 360���� bulletCount�� ����� 45�� �������� �߻�

    //    for (int i = 0; i < bulletCount; i++)
    //    {
    //        float angle = i * angleStep * Mathf.Deg2Rad; //���� ������ ��ȯ
    //        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); //Cos�� x�� ����, Sin�� y�� ���� �̰ɷ� unit circle���� ������ ���

    //    }
    //    yield return null;
    //}
    private void OnEnable()
    {
        target = PlayerController.Instance.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            bossManager.Health -= Player.Instance.power;
            Debug.Log(bossManager.Health);
            if (bossManager.Health <= 0)
            {
                // ��ų ������Ʈ ����
                foreach (GameObject obj in activeSkillObjects)
                {
                    if (obj != null && obj.activeSelf)
                    {
                        BossObjectPoolManager.Instance.ReturnToPool(obj.tag, obj);
                    }
                }
                activeSkillObjects.Clear();

                Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(gameObject);  // ���� ������Ʈ �ı�
            }
        }
    }
}
