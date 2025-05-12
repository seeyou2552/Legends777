using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int bossHealth = 10;
    public int Health
    {
        get => bossHealth;
        set => bossHealth = Mathf.Clamp(value, 0, 1000);
    }

    [Range(1f, 20f)][SerializeField] private float bossSpeed = 3f;
    public float MonsterSpeed
    {
        get => bossSpeed;
        set => bossSpeed = Mathf.Clamp(value, 0, 20);
    }

    private Rigidbody2D target; // 타겟 확인
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
        if (instance == null) instance = this;
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Boss_Camera = Camera.main;
    }

    

    private void FixedUpdate()
    {
        if(target == null) return;
        Vector2 direction = target.position - rigid.position; // 타겟과 몬스터의 위치 길이 구하기

        float stopDistance = 1.5f; // 타겟과의 최소 거리

        if (direction.magnitude >= stopDistance)
        {
            Vector2 nextDir = direction.normalized * bossSpeed * Time.fixedDeltaTime; // 플레이어에게 향하는 움직임
            rigid.MovePosition(rigid.position + nextDir); // 움직임 구현
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
        spriter.flipX = target.position.x < rigid.position.x; // 몬스터가 플레이어보다 x위치가 다르면 방향 변경
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
                // 스킬 오브젝트 정리
                foreach (GameObject obj in BossSkillManager.Instance.ActiveSkillObjects)
                {
                    if (obj != null && obj.activeSelf)
                    {
                        BossObjectPoolManager.Instance.ReturnToPool(obj.tag, obj);
                    }
                }
                BossSkillManager.Instance.ActiveSkillObjects.Clear();

                Boss_Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(gameObject);  // 보스 오브젝트 파괴
            }
        }
    }
}
