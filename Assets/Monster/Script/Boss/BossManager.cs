using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int initialHealth = 10;
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

    SpriteRenderer spriter;
    Animator animator;

    private bool isHalf = false;
    public GameObject redBackGround;
    private Camera Boss_Camera;

    public static BossManager instance;

    private void Awake()
    {
        MaxHealth = initialHealth;
        Health = MaxHealth;

        if (instance == null) instance = this;
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Boss_Camera = Camera.main;
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
        playerTarget = Player.Instance.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            Health -= Player.Instance.power;
            animator.SetTrigger("isHit");
            Debug.Log(Health);

            if (BossManager.instance.Health <= 500 && !isHalf)
            {
                CameraShake.instance.StartShake();
                Instantiate(redBackGround, transform);
                isHalf = true;
            }
            if (Health <= 0)
            {
                // ��ų ������Ʈ ����
                foreach (GameObject obj in BossSkillManager.Instance.ActiveSkillObjects)
                {
                    if (obj != null && obj.activeSelf)
                    {
                        BossObjectPoolManager.Instance.ReturnToPool(obj.tag, obj);
                    }
                }
                BossSkillManager.Instance.ActiveSkillObjects.Clear();

                GameManager.instance.IsStageClear = true;

                Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(gameObject);  // ���� ������Ʈ �ı�
                QuestManager.Instance.QuestCheck(1);
            }
        }
    }
}
