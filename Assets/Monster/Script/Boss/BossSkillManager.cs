using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BossSkillManager : MonoBehaviour
{
    public static BossSkillManager Instance;

    public Transform firePoint; // 氤挫姢 ?勳箻 臧�?胳槫旮?
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

    private List<GameObject> activeSkillObjects = new List<GameObject>(); // ?半 ?濎劚?橂姅 ?る笇?濏姼毳?氤挫姢臧� 欤届潉????牅?橁赴 ?勴暣 ?�?ロ暣?愲姅 毽姢??
    public List<GameObject> ActiveSkillObjects => activeSkillObjects; // ?胳姢?欗劙 彀届棎???堧炒?搓碃 ?ろ伂毽巾姼?愳劀 彀胳“?犾垬?堦矊
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
        gameManager.OnStageUpdated += SkillsForStage; // ?ろ厡?挫? ?措菠???毄

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
    private void SkillsForStage() // ?毄???ろ偓?彪?垬
    {
        int stage = gameManager.Stage;
        skillFuncs.Clear();

        // ?毄???ろ偓???彪
        if (gameManager.Stage < 3)
        {
            //skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            //skillFuncs.Add(LazerPatten2);
            //skillFuncs.Add(CameraReversal);
            //skillFuncs.Add(ShootFast);
            //skillFuncs.Add(Teleport);
            //skillFuncs.Add(MakeBossItem);
            //skillFuncs.Add(LazerPattern);
            //skillFuncs.Add(RedGround);
        }
        else if (gameManager.Stage < 5)
        {
            //skillFuncs.Add(MoveFast);
            skillFuncs.Add(MakeMonster);
            //skillFuncs.Add(LazerPatten2);
            //skillFuncs.Add(CameraReversal);
            //skillFuncs.Add(ShootFast);
            //skillFuncs.Add(Teleport);
            //skillFuncs.Add(MakeBossItem);
            //skillFuncs.Add(LazerPattern);
            //skillFuncs.Add(RedGround);
        }
        else if (gameManager.Stage < 9)
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

        // ?る笇?濏姼 ?�?愳劀 ?岇澊?措臣 旰茧偞旮?
        GameObject fireball = BossObjectPoolManager.Instance.GetFromPool("Fireball", spawnPos, Quaternion.identity/*, this.transform*/);
        if (fireball == null) // ?岅创???る笇?濏姼??瓴届毎
        {
            return; // ?措嫻 ?る笇?濏姼?????挫儊 ?毄?????嗢溂氙�搿?膦呺
        }

        // 於╇弻 觳橂Μ - 氤挫姢?� ?岇澊?措臣??於╇弻?橃? ?婋弰搿??れ爼
        Collider2D bossCol = GetComponent<Collider2D>();
        Collider2D fireballCol = fireball.GetComponent<Collider2D>();
        if (fireballCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(fireballCol, bossCol);
        }

        // ?岇澊?措臣???ろ伂毽巾姼 ?瓣舶 氚?氚╉枼, ?嶋弰 ?れ爼
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
            int bulletCount = 8; // 氚滌偓???岇澊?措臣??臧滌垬
            float angleStep = 360f / bulletCount; //齑?360?勲? bulletCount搿??橂垊?挫劀 45??臧勱博?茧 氚滌偓

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad; //?茧敂??臧侂弰搿?氤�??
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); //Cos??x於?氚╉枼, Sin?� y於?氚╉枼 ?搓备搿?unit circle?侅潣 氚╉枼??瓿勳偘
                ShootFireball(direction);
            }
        }
    }

    //?岆爤?挫柎臧� 臁挫灛?橂姅歆� 觳错伂?橁碃 ?ろ偓?毄 ?犽 ?愲嫧
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

                    } while (skillFuncs[currentSkillIndex] == skillFuncs[currentskillIndex2]);
                    var nextskill2 = skillFuncs[currentskillIndex2];
                    if (CanUseSkill(nextskill2))
                    {
                        StartCoroutine(nextSkill());
                        StartCoroutine(nextskill2());
                        yield return new WaitForSeconds(5f);
                    }
                }
                if (CanUseSkill(nextSkill))
                {
                    yield return StartCoroutine(nextSkill());
                    yield return new WaitForSeconds(2f);
                }
            }
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
        // 罚待茄 利 橇府普 急琶
        GameObject randomPrefab = MonsterManager.Instance.enemyPrefabs[UnityEngine.Random.Range(0, MonsterManager.Instance.enemyPrefabs.Count)];

        // 罚待茄 康开 急琶
        Vector2 randomPosition = new Vector2(
            UnityEngine.Random.Range(-8, 9),
            UnityEngine.Random.Range(-4, 3.5f)
        );

        // 利 积己 棺 府胶飘俊 眠啊
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        MonsterController enemyController = spawnedEnemy.GetComponent<MonsterController>();
        enemyController.Init();

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
        GameObject red = BossObjectPoolManager.Instance.GetFromPool("redGround", new Vector2(0,-0.6f), Quaternion.identity/*, this.transform*/);
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
            GameObject lazer1 = BossObjectPoolManager.Instance.GetFromPool("redLazer1", Vector2.zero, Quaternion.identity/*, this.transform*/);
            if(!activeSkillObjects.Contains(lazer1))
            {
                activeSkillObjects.Add(lazer1);
            }
            yield return new WaitForSeconds(0.1f);
            BossObjectPoolManager.Instance.ReturnToPool("redLazer1", lazer1);

            GameObject lazer2 = BossObjectPoolManager.Instance.GetFromPool("redLazer2", Vector2.zero, Quaternion.identity/*, this.transform*/);
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
            GameObject rLazer = BossObjectPoolManager.Instance.GetFromPool("rLazer", new Vector2(x, y), Quaternion.Euler(0, 0, z)/*, this.transform*/);
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
            float x = UnityEngine.Random.Range(-9, 9f);
            float y = UnityEngine.Random.Range(-4, 4f);
            teleport[i] = BossObjectPoolManager.Instance.GetFromPool("Teleport", new Vector2(x, y), Quaternion.identity/*, this.transform*/);
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

    private IEnumerator MakeBossItem()
    {
        GameObject[] item = new GameObject[3];
        for(int i = 0;i < 3;i++)
        {
            float x = UnityEngine.Random.Range(-9, 9f);
            float y = UnityEngine.Random.Range(-4, 4f);
            item[i] = BossObjectPoolManager.Instance.GetFromPool("item", new Vector2(x, y), Quaternion.identity/*, this.transform*/);
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