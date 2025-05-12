using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSkillManager : MonoBehaviour
{
    public static BossSkillManager Instance;

    public Transform firePoint; // ���� ��ġ ��������
    private Transform target;
    private Camera Boss_Camera;

    public float fireRate = 3f;
    public float defaultBulletSpeed = 15f;

    private float currentBulletSpeed;
    private float nextFireTime = 0f;
    private int attackCount = 1;

    private List<Func<IEnumerator>> skillFuncs = new List<Func<IEnumerator>>();
    private int currentSkillIndex = 0;

    private List<GameObject> activeSkillObjects = new List<GameObject>(); // ���� �����Ǵ� ������Ʈ�� ������ ������ �����ϱ� ���� �����صδ� ����Ʈ
    public List<GameObject> ActiveSkillObjects => activeSkillObjects; // �ν����� â���� �Ⱥ��̰� ��ũ��Ʈ���� �����Ҽ��ְ�

    BossManager bossManager;
    GameManager gameManager;
    Player player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.Instance;
        gameManager = GameManager.instance;
        bossManager = GetComponent<BossManager>();
        Boss_Camera = Camera.main;
        currentBulletSpeed = defaultBulletSpeed;

        SkillsForStage();
        gameManager.OnStageUpdated += SkillsForStage; // �������� �̺�Ʈ ���

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
    private void SkillsForStage() // ����� ��ų����Լ�
    {
        int stage = gameManager.Stage;
        skillFuncs.Clear();

        // ����� ��ų�� ���
        if (gameManager.Stage <= 3)
        {
            skillFuncs.Add(MakeBossItem);
            skillFuncs.Add(MoveFast);
            //skillFuncs.Add(LazerPatten2);
            //skillFuncs.Add(MakeMonster);
        }
        else if (gameManager.Stage < 5)
        {
            skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            skillFuncs.Add(LazerPatten2);
            skillFuncs.Add(CameraReversal);
            skillFuncs.Add(ShootFast);
            skillFuncs.Add(Teleport);
        }
        else if (gameManager.Stage < 7)
        {
            skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            skillFuncs.Add(LazerPatten2);
            skillFuncs.Add(CameraReversal);
            skillFuncs.Add(ShootFast);
            skillFuncs.Add(Teleport);
            skillFuncs.Add(MakeBossItem);
            skillFuncs.Add(LazerPattern);
            skillFuncs.Add(RedGround);
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

    //�÷��̾ �����ϴ��� üũ�ϰ� ��ų��� ���� �Ǵ�
    private bool CanUseSkill(Func<IEnumerator> _) => BossManager.instance.PlayerTarget != null;

    private IEnumerator UseSkillsRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (skillFuncs.Count > 0)
            {
                var nextSkill = skillFuncs[currentSkillIndex];

                if (CanUseSkill(nextSkill))
                {
                    yield return StartCoroutine(nextSkill());
                }

                int num = UnityEngine.Random.Range(0, skillFuncs.Count);
                currentSkillIndex = (currentSkillIndex + num) % skillFuncs.Count;
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator MoveFast()
    {
        bossManager.MonsterSpeed = 6;
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
        fireRate = 6;
        currentBulletSpeed = 25f;
        yield return new WaitForSeconds(5f);
        currentBulletSpeed = defaultBulletSpeed;
        fireRate = 3;
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

    private IEnumerator LazerPatten2()
    {
        for (int i = 0; i < 10; i++)
        {
            float x = UnityEngine.Random.Range(-9, 9.1f);
            float y = UnityEngine.Random.Range(-4, 4.1f);
            float z = UnityEngine.Random.Range(-180, 180);
            GameObject rLazer = BossObjectPoolManager.Instance.GetFromPool("rLazer", new Vector2(x, y), Quaternion.Euler(0, 0, z));
            if (!activeSkillObjects.Contains(rLazer))
            {
                activeSkillObjects.Add(rLazer);
            }
            SpriteRenderer sr = rLazer.GetComponent<SpriteRenderer>();
            Color color = sr.color;

            for(int j = 0; j < 5; j++)
            {
                color.a -= 0.2f;
                sr.color = color;
                yield return new WaitForSeconds(0.1f);
            }
            color.a = 1f;
            sr.color = color;
            BossObjectPoolManager.Instance.ReturnToPool("rLazer", rLazer);
        }
    }

    private IEnumerator Teleport()
    {
        GameObject[] teleport = new GameObject[2];
        for (int i = 0; i < 2; i++)
        {
            float x = UnityEngine.Random.Range(-9, 9.1f);
            float y = UnityEngine.Random.Range(-4, 4.1f);
            teleport[i] = BossObjectPoolManager.Instance.GetFromPool("Teleport", new Vector2(x, y), Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 2; i++)
        {
            BossObjectPoolManager.Instance.ReturnToPool("Teleport", teleport[i]);
        }
    }

    private IEnumerator MakeBossItem()
    {
        GameObject[] item = new GameObject[3];
        for(int i = 0;i < 3;i++)
        {
            float x = UnityEngine.Random.Range(-9, 9.1f);
            float y = UnityEngine.Random.Range(-4, 4.1f);
            item[i] = BossObjectPoolManager.Instance.GetFromPool("item", new Vector2(x, y), Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 3; i++)
        {
            if (item[i].activeSelf)
            {
                BossObjectPoolManager.Instance.ReturnToPool("item", item[i]);
            }
        }
    }

    public IEnumerator MakeRandomEffect()
    {
        int random = UnityEngine.Random.Range(0, 4);

        Action ApplyEffect = null;
        Action RemoveEffect = null;
        float duration = 5f;
        switch (random)
        {
            case 0:
                ApplyEffect = () => { player.attackSpeed *= 2; Debug.Log("+asp"); };
                RemoveEffect = () => { player.attackSpeed /= 2; };
                break;
            case 1:
                ApplyEffect = () => { player.speed *= 2; Debug.Log("+sp"); };
                RemoveEffect = () => { player.speed /= 2; };
                break;
            case 2:
                ApplyEffect = () => { player.hp *= 2; Debug.Log("+hp"); };
                duration = 0f;
                break;
            case 3:
                ApplyEffect = () => { player.power *= 2; Debug.Log("+po"); };
                RemoveEffect = () => { player.power /= 2; };
                break;
            default:
                Debug.LogError("noEffect");
                break;
        }

        ApplyEffect?.Invoke();

        if (duration > 0 && RemoveEffect != null)
        {
            yield return new WaitForSeconds(duration);
            RemoveEffect();
        }
    }

    public IEnumerator MakeRandomBedEffect()
    {
        int random = UnityEngine.Random.Range(0, 4);

        Action ApplyEffect = null;
        Action RemoveEffect = null;
        float duration = 5f;
        switch (random)
        {
            case 0:
                ApplyEffect = () => { player.attackSpeed /= 2; };
                RemoveEffect = () => { player.attackSpeed *= 2; };
                break;
            case 1:
                ApplyEffect = () => { player.speed /= 2; Debug.Log("-sp"); };
                RemoveEffect = () => { player.speed *= 2; };
                break;
            case 2:
                ApplyEffect = () => { player.hp /= 2; Debug.Log("-hp"); };
                duration = 0f;
                break;
            case 3:
                ApplyEffect = () => { player.power /= 2; Debug.Log("-po"); };
                RemoveEffect = () => { player.power *= 2; };
                break;
            default:
                Debug.LogError("noBedEffect");
                break;
        }

        ApplyEffect?.Invoke();

        if (duration > 0 && RemoveEffect != null)
        {
            yield return new WaitForSeconds(duration);
            RemoveEffect();
        }
    }
    private void OnEnable()
    {
        if (PlayerController.Instance != null)
            target = PlayerController.Instance.transform;
    }
}
