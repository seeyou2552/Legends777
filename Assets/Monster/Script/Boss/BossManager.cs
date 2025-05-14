using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class BossManager : MonoBehaviour
{
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip deathSound;
    [Range(1, 5000)][SerializeField] private int initialHealth = 10;
    public int MaxHealth { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action Ondead;

    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke(health, MaxHealth);
            if(health <= 0)
            {
                Ondead?.Invoke();
                Destroy(gameObject);
            }
        }

    }

    [Range(1f, 20f)][SerializeField] private float bossSpeed = 3f;
    public float MonsterSpeed
    {
        get => bossSpeed;
        set => bossSpeed = Mathf.Clamp(value, 0, 20);
    }

    private Rigidbody2D target; // Ÿ�� Ȯ��
    public Rigidbody2D Target => target;

    private Rigidbody2D rigid;
    public Rigidbody2D Rigid => rigid;

    private GameObject playerTarget;
    public GameObject PlayerTarget => playerTarget;

    public GameObject bossObject;

    SpriteRenderer spriter;
    Animator animator;

    private bool isHalf = false;
    public GameObject redBackGround;
    private Camera Boss_Camera;
    public int firstHp;

    public static BossManager instance;

    private void Awake()
    {
        MaxHealth = initialHealth;
        Health = MaxHealth;

        if (instance == null) instance = this;

        bossObject = this.gameObject;

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Boss_Camera = Camera.main;
        SoundManager.Instance.PlaySFX(spawnSound);
    }

    private void Start()
    {
        firstHp = Health;
    }

    private void FixedUpdate()
    {
        if(target == null) return;
        Vector2 direction = target.position - rigid.position; // Ÿ�ٰ� ������ ��ġ ���� ���ϱ�

        float stopDistance = 1.5f; // Ÿ�ٰ��� �ּ� �Ÿ�

        if (direction.magnitude >= stopDistance)
        {
            Vector2 nextDir = direction.normalized * bossSpeed * Time.fixedDeltaTime; // �÷��̾�� ���ϴ� ������
            rigid.MovePosition(rigid.position + nextDir); // ������ ����
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;
        spriter.flipX = target.position.x < rigid.position.x; // ���Ͱ� �÷��̾�� x��ġ�� �ٸ��� ���� ����
    }

    private void OnEnable()
    {
        target = PlayerController.Instance.GetComponent<Rigidbody2D>();
        playerTarget = PlayerController.Instance.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            if(PlayerController.Instance.Equip != null) { 
                Health -= PlayerController.Instance.power + PlayerController.Instance.Equip.Atk(); }
            else { 
                Health -= PlayerController.Instance.power + 5; }
            
            animator.SetTrigger("isHit");

            if (Health <= firstHp / 2 && !isHalf)
            {
                FindObjectOfType<CameraController>().StartShake();
                Instantiate(redBackGround, transform);
                isHalf = true;
            }
            if (Health <= 0)
            {
                foreach (GameObject obj in BossSkillManager.Instance.ActiveSkillObjects)
                {
                    var pooled = obj.GetComponent<PoolTag>();
                    if (obj != null && obj.activeSelf)
                    {
                        BossObjectPoolManager.Instance.ReturnToPool(pooled.poolTag, obj);
                    }
                }
                BossSkillManager.Instance.ActiveSkillObjects.Clear();
                QuestManager.Instance.QuestCheck(1);

                PlayerController.Instance.attackSpeed =  BossSkillManager.Instance.playerAtkSpeed;
                PlayerController.Instance.speed = BossSkillManager.Instance.playerSpeed;
                PlayerController.Instance.power = BossSkillManager.Instance.playerPower;

                GameManager.instance.IsStageClear = true;

                Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(gameObject);  // ���� ������Ʈ �ı�
                SoundManager.Instance.PlaySFX(deathSound);
            }
        }
    }
}