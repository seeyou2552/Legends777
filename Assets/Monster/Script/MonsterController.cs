using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    private MonsterStatHandler monsterStatHandler;


    //Ÿ�� Ȯ��
    public Rigidbody2D target;
    //Ÿ���� �Ѿư� �ִ� �Ÿ�
    private float followRange = 15f;

    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


    //���� ����
    private bool isDead;

    private Vector2 knockback = Vector2.zero;   //�˹� ����
    private float knockbackDuration = 0.0f;     //�˹� ���� �ð�3

    protected bool isAttacking;                                 //���� �� ����
    private float timeSinceLastAttack = 0;         //������ ���� ���� ��� �ð�

    //����
    [SerializeField] public Transform weaponPivot;
    [SerializeField] public MonsterWeaponHandler WeaponPrefab;         //������ ���� ������(������ �ڽĿ��� ã�Ƽ� ���)
    [SerializeField] protected MonsterWeaponHandler weaponHandler;                      //������ ����

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    MonsterManager monsterManager;

    private void Awake()
    {
        monsterManager = MonsterManager.Instance;

        monsterStatHandler = GetComponent<MonsterStatHandler>();

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<MonsterWeaponHandler>();
    }

    public void Init()
    {
        isDead = false;
        target = PlayerController.Instance.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAction();
        //ȸ�� ó��
        Rotate(lookDirection);
        //���� �Է� �� ��Ÿ�� ����
        HandleAttackDelay();
    }


    private float DistanceToTarget()
    {
        return Vector3.Distance(target.position, rigid.position);
    }

    private Vector2 DirectionToTarget()
    {
        return (target.position - rigid.position).normalized;
    }

    private void HandleAction()
    {
        if (weaponHandler == null || target == null)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        //Ÿ�ٱ��� �Ÿ�
        float distance = DistanceToTarget();
        //Ÿ�� ���� (����)
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        //�÷��̾ ���� �Ÿ� �ȿ� ���� ���� ���� ����
        if (distance <= followRange)
        {
            //����
            lookDirection = direction;

            //���� ��Ÿ� ������ ������ ���
            if (distance < weaponHandler.AttackRange)
            {
                isAttacking = true;
                //���� ���� ���̹Ƿ� ����
                rigid.velocity = Vector2.zero;
                return;
            }

            //���� ���� ���� �ƴϱ� ������ �̵�
            // �÷��̾�� ���ϴ� ������
            Vector2 nextDir = direction * monsterStatHandler.Speed * Time.fixedDeltaTime;
            // ������ ���� - ���Ÿ��� �������� �ʰ�
            if (gameObject.name.Contains("Far"))
            {
                rigid.velocity = Vector2.zero;
            }
            else
            {
                rigid.MovePosition(rigid.position + nextDir);
                animator.SetBool("isRun", true);
            }
        }
        else
        {
            //�÷��̾ ���� ���� �ۿ� �ִٸ� ������ �־�� ��
            rigid.velocity = Vector2.zero;
            animator.SetBool("isRun", false);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; //90������ ũ�ٸ� ������ �ٶ󺸴� ����

        //ĳ���Ͱ� ���� ������ �̹��� ������
        spriter.flipX = isLeft;

        if (weaponPivot != null)
        {
            //weaponPivot�� z�� �������� rotZ��ŭ ȸ��
            //���� ȸ�� ó��
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        //���⵵ �¿� ���� ó��
        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {

        if (weaponHandler == null)
        {
            return;
        }

        //������ ��ٿ� ���̸� �ð� ����
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //���� �ð����� �߻�
        //������ �Է� ���̰� ��Ÿ���� �������� ���� ����
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            //���� ���� ����
            Attack();
        }
    }

    protected virtual void Attack()
    {
        //�ٶ󺸴� ������ ���� ���� ����
        if (lookDirection != Vector2.zero)
            weaponHandler.Attack();
    }

    #region Damage ó��
    public void OnDamaged(int damage)
    {
        monsterStatHandler.Health -= damage;

        if (monsterStatHandler.Health <= 0)
        {
            isDead = true;
            OnDead();
        }
    }

    public void OnDead()
    {
        GameManager.instance.KillCount++;

        //������ ����
        rigid.velocity = Vector2.zero;

        //��� SpriteRenderer�� ���� ���缭 ���� ȿ�� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        //��� ������Ʈ(��ũ��Ʈ ����) ��Ȱ��ȭ
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        //2�� �� ������Ʈ �ı�
        Destroy(gameObject, 2f);
    }
    #endregion

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        //�ش� ���⿡�� ����� ����ü ������ ��������
        GameObject origin = monsterManager.projectilePrefabs[rangeWeaponHandler.BulletIndex];

        //������ ��ġ�� ����ü ����
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);

        //����ü�� �ʱ� ���� ���� (����, ���� ������)
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }
}
