using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BossSkillManager : MonoBehaviour
{
    public static BossSkillManager Instance;

    public Transform firePoint; // 보스 기본 공격 생성 위치 구하기
    private Transform target;
    private Camera Boss_Camera;
    public TextMeshProUGUI EffectText;

    public float fireRate = 3f;
    public float defaultBulletSpeed = 15f;

    private float currentBulletSpeed;
    private float nextFireTime = 0f;
    private int attackCount = 1;

    private List<Func<IEnumerator>> skillFuncs = new List<Func<IEnumerator>>();
    private int currentSkillIndex = 0;
    private int currentskillIndex2 = 0;

    private List<GameObject> activeSkillObjects = new List<GameObject>(); // 보스가 사용한 스킬을 저장하는 리스트
    public List<GameObject> ActiveSkillObjects => activeSkillObjects; // 참조는 가능하지만 인스펙터 창에서 보이지 않게 하기위한것
    public float playerSpeed;
    public float playerAtkSpeed;
    public int playerPower;

    BossManager bossManager;
    GameManager gameManager;
    PlayerController player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = PlayerController.Instance;
        gameManager = GameManager.instance;
        bossManager = GetComponent<BossManager>();
        Boss_Camera = Camera.main;
        currentBulletSpeed = defaultBulletSpeed;

        playerSpeed = player.speed;
        playerPower = player.power;
        playerAtkSpeed = player.attackSpeed;

        SkillsForStage();
        gameManager.OnStageUpdated += SkillsForStage; // 게임 매니저의 이벤트 활용

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
    private void SkillsForStage()
    {
        int stage = gameManager.Stage;
        skillFuncs.Clear();

        if (gameManager.Stage < 2)
        {
            skillFuncs.Add(RedGround);
            skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            skillFuncs.Add(LazerPatten2);
        }
        else if (gameManager.Stage < 3)
        {
            skillFuncs.Add(RedGround);
            skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            skillFuncs.Add(LazerPatten2);
            skillFuncs.Add(CameraReversal);
            skillFuncs.Add(ShootFast);
            skillFuncs.Add(Teleport);
        }
        else if (gameManager.Stage < 50)
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

        GameObject fireball = BossObjectPoolManager.Instance.GetFromPool("Fireball", spawnPos, Quaternion.identity);
        if (fireball == null) // 파이어볼이 없으면 리턴
        {
            return;
        }

        //
        Collider2D bossCol = GetComponent<Collider2D>();
        Collider2D fireballCol = fireball.GetComponent<Collider2D>();
        if (fireballCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(fireballCol, bossCol);
        }

        // 
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
            int bulletCount = 8; // 발사할 파이어볼 개수
            float angleStep = 360f / bulletCount; //각도 구하기

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;// 각도를 라디안으로 변환
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // 방향 벡터 계산
                ShootFireball(direction);
            }
        }
    }

    //보스가 스킬을 사용할수있는 상태인지 체크
    private bool CanUseSkill(Func<IEnumerator> _) => BossManager.instance.PlayerTarget != null;

    private IEnumerator UseSkillsRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (skillFuncs.Count > 0)
            {
                int num = UnityEngine.Random.Range(0, skillFuncs.Count);
                currentSkillIndex = (currentSkillIndex + num) % skillFuncs.Count;
                var nextSkill = skillFuncs[currentSkillIndex];
                if (bossManager.Health <= bossManager.firstHp / 2 && CanUseSkill(nextSkill))
                {
                    do
                    {
                        int num2 = UnityEngine.Random.Range(0, skillFuncs.Count);
                        currentskillIndex2 = (currentSkillIndex + num2) % skillFuncs.Count;

                    } while (skillFuncs[currentSkillIndex] == skillFuncs[currentskillIndex2] || !CanUseSkill(nextSkill));
                    var nextskill2 = skillFuncs[currentskillIndex2];
                    if (CanUseSkill(nextskill2))
                    {
                        StartCoroutine(nextSkill());
                        StartCoroutine(nextskill2());
                        yield return new WaitForSeconds(5f);
                    }
                }
                else if (CanUseSkill(nextSkill))
                {
                    yield return StartCoroutine(nextSkill());
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }

    private IEnumerator MoveFast() // 보스 이동 속도 증가
    {
        bossManager.MonsterSpeed = 6;
        yield return new WaitForSeconds(5f);
        bossManager.MonsterSpeed = 3;
    }

    private IEnumerator MakeMonster()
    {
        // 몬스터 프리팹 가져오기
        GameObject randomPrefab = MonsterManager.Instance.enemyPrefabs[UnityEngine.Random.Range(0, MonsterManager.Instance.enemyPrefabs.Count)];

        // 생성 위치 정하기
        Vector2 randomPosition = new Vector2(
            UnityEngine.Random.Range(-8, 9),
            UnityEngine.Random.Range(-4, 3.5f)
        );

        // 몬스터 생성
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        MonsterController enemyController = spawnedEnemy.GetComponent<MonsterController>();
        enemyController.Init();

        yield return null;
    }

    private IEnumerator CameraReversal() // 카메라 180도 회전
    {
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return new WaitForSeconds(5f);
        Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private IEnumerator ShootFast() // 보스 공격 속도 증가
    {
        fireRate = 6;
        currentBulletSpeed = 25f;
        yield return new WaitForSeconds(5f);
        currentBulletSpeed = defaultBulletSpeed;
        fireRate = 3;
    }

    private IEnumerator RedGround() // 빨간 장판 설치 
    {
        GameObject red = BossObjectPoolManager.Instance.GetFromPool("redGround", new Vector2(0,-0.6f), Quaternion.identity);
        if (!activeSkillObjects.Contains(red))
        {
            activeSkillObjects.Add(red);
        }
        yield return new WaitForSeconds(5f);
        BossObjectPoolManager.Instance.ReturnToPool("redGround", red);
    }

    private IEnumerator LazerPattern() // x, + 레이저 패턴 
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
            float x = UnityEngine.Random.Range(-22, 22.1f);
            float y = UnityEngine.Random.Range(-10, 10.1f);
            teleport[i] = BossObjectPoolManager.Instance.GetFromPool("Teleport", new Vector2(x, y), Quaternion.identity);
            if (!activeSkillObjects.Contains(teleport[i]))
            {
                activeSkillObjects.Add(teleport[i]);
            }
        }
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 2; i++)
        {
            BossObjectPoolManager.Instance.ReturnToPool("Teleport", teleport[i]);
        }
    }

    private IEnumerator MakeBossItem() // 랜덤 효과 아이템 패턴
    {
        GameObject[] item = new GameObject[3];
        for(int i = 0;i < 3;i++)
        {
            float x = UnityEngine.Random.Range(-22, 22.1f);
            float y = UnityEngine.Random.Range(-10, 10.1f);
            item[i] = BossObjectPoolManager.Instance.GetFromPool("item", new Vector2(x, y), Quaternion.identity);
            if (!activeSkillObjects.Contains(item[i]))
            {
                activeSkillObjects.Add(item[i]);
            }
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
        int random = UnityEngine.Random.Range(0, 3);

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
                ApplyEffect = () => { player.power *= 2; Debug.Log("+po"); };
                RemoveEffect = () => { player.power /= 2; };
                break;
        }

        ApplyEffect?.Invoke();

        if (RemoveEffect != null)
        {
            yield return new WaitForSeconds(duration);
            RemoveEffect();
        }
    }

    public IEnumerator MakeRandomBedEffect()
    {
        int random = UnityEngine.Random.Range(0, 3);

        Action ApplyEffect = null;
        Action RemoveEffect = null;
        float duration = 5f;
        switch (random)
        {
            case 0:
                ApplyEffect = () => { player.attackSpeed /= 2; Debug.Log("-asp"); };
                RemoveEffect = () => { player.attackSpeed *= 2; };
                break;
            case 1:
                ApplyEffect = () => { player.speed /= 2; Debug.Log("-sp"); };
                RemoveEffect = () => { player.speed *= 2; };
                break;
            case 2:
                ApplyEffect = () => { player.power /= 2; Debug.Log("-po"); };
                RemoveEffect = () => { player.power *= 2; };
                break;
        }

        ApplyEffect?.Invoke();

        if (RemoveEffect != null)
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