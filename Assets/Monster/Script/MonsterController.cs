using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    private MonsterStatHandler monsterStatHandler;


    //?��??�인
    public Rigidbody2D target;
    //?�겟??쫓아�?최�? 거리
    private float followRange = 15f;

    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


    //죽음 ?�무
    private bool isDead;

    private Vector2 knockback = Vector2.zero;   //?�백 방향
    private float knockbackDuration = 0.0f;     //?�백 지???�간3

    protected bool isAttacking;                                 //공격 �??��?
    private float timeSinceLastAttack = 0;         //마�?�?공격 ?�후 경과 ?�간

    //무기
    [SerializeField] public Transform weaponPivot;
    [SerializeField] public MonsterWeaponHandler WeaponPrefab;         //?�착??무기 ?�리???�으�??�식?�서 찾아???�용)
    [SerializeField] protected MonsterWeaponHandler weaponHandler;                      //?�착??무기

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
        //?�전 처리
        Rotate(lookDirection);
        //공격 ?�력 �?쿨�???관�?
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

        //?�겟까�? 거리
        float distance = DistanceToTarget();
        //?��?방향 (무기)
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        //?�레?�어가 ?�정 거리 ?�에 ?�을 ?�만 추적 ?�작
        if (distance <= followRange)
        {
            //방향
            lookDirection = direction;

            //공격 ?�거�??�으�??�어?�을 경우
            if (distance < weaponHandler.AttackRange)
            {
                isAttacking = true;
                //공격 범위 ?�이므�??��?
                rigid.velocity = Vector2.zero;
                return;
            }

            //공격 범위 ?�이 ?�니�??�문???�동
            // ?�레?�어?�게 ?�하???�직??
            Vector2 nextDir = direction * monsterStatHandler.Speed * Time.fixedDeltaTime;

            rigid.MovePosition(rigid.position + nextDir);
            animator.SetBool("IsRun", true);
        }
        else
        {
            //?�레?�어가 추적 범위 밖에 ?�다�?가만히 ?�어????
            rigid.velocity = Vector2.zero;
            animator.SetBool("IsRun", false);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; //90?�보???�다�??�쪽??바라보는 것임

        //캐릭?��? 보는 방향?��??��?지 ?�집�?
        spriter.flipX = isLeft;

        if (weaponPivot != null)
        {
            //weaponPivot??z�?기�??�로 rotZ만큼 ?�전
            //무기 ?�전 처리
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        //무기??좌우 반전 처리
        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {

        if (weaponHandler == null)
        {
            return;
        }

        //공격??쿨다??중이�??�간 ?�적
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //?�정 ?�간마다 발사
        //공격???�력 중이�?쿨�??�이 ?�났?�면 공격 ?�행
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            //?�제 공격 ?�행
            Attack();
        }
    }

    protected virtual void Attack()
    {
        //바라보는 방향???�을 ?�만 공격
        if (lookDirection != Vector2.zero)
            weaponHandler.Attack();
    }

    #region Damage 처리
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
        //monsterManager.RemoveActiveMonster(this);

        //?�직???��?
        rigid.velocity = Vector2.zero;

        //모든 SpriteRenderer???�명????��??죽�? ?�과 ?�출
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        //모든 컴포?�트(?�크립트 ?�함) 비활?�화
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        QuestManager.Instance.QuestCheck(0);
        monsterManager.RemoveActiveMonster(this);

        //2�????�브?�트 ?�괴
        Destroy(gameObject, 2f);
    }
    #endregion

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        //?�당 무기?�서 ?�용???�사�??�리??가?�오�?
        GameObject origin = monsterManager.projectilePrefabs[rangeWeaponHandler.BulletIndex];

        //지?�된 ?�치???�사�??�성
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);

        //?�사체에 초기 ?�보 ?�달 (방향, 무기 ?�이??
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }
}