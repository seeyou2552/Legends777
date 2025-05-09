using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    private MonsterStatHandler monsterStatHandler;


    //타겟 확인
    public Rigidbody2D target;
    //타겟을 쫓아갈 최대 거리
    private float followRange = 15f;

    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


    //죽음 유무
    private bool isDead;

    private Vector2 knockback = Vector2.zero;   //넉백 방향
    private float knockbackDuration = 0.0f;     //넉백 지속 시간3

    protected bool isAttacking;                                 //공격 중 여부
    private float timeSinceLastAttack = 0;         //마지막 공격 이후 경과 시간

    //무기
    [SerializeField] public Transform weaponPivot;
    [SerializeField] public MonsterWeaponHandler WeaponPrefab;         //장착할 무기 프리팹(없으면 자식에서 찾아서 사용)
    [SerializeField] protected MonsterWeaponHandler weaponHandler;                      //장착된 무기

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
        //회전 처리
        Rotate(lookDirection);
        //공격 입력 및 쿨타임 관리
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

        //타겟까지 거리
        float distance = DistanceToTarget();
        //타겟 방향 (무기)
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        //플레이어가 일정 거리 안에 있을 때만 추적 시작
        if (distance <= followRange)
        {
            //방향
            lookDirection = direction;

            //공격 사거리 안으로 들어왔을 경우
            if (distance < weaponHandler.AttackRange)
            {
                isAttacking = true;
                //공격 범위 안이므로 정지
                rigid.velocity = Vector2.zero;
                return;
            }

            //공격 범위 안이 아니기 때문에 이동
            // 플레이어에게 향하는 움직임
            Vector2 nextDir = direction * monsterStatHandler.Speed * Time.fixedDeltaTime;
            // 움직임 구현 - 원거리는 움직이지 않게
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
            //플레이어가 추적 범위 밖에 있다면 가만히 있어야 함
            rigid.velocity = Vector2.zero;
            animator.SetBool("isRun", false);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; //90도보다 크다면 왼쪽을 바라보는 것임

        //캐릭터가 보는 방향대로 이미지 뒤집기
        spriter.flipX = isLeft;

        if (weaponPivot != null)
        {
            //weaponPivot을 z축 기준으로 rotZ만큼 회전
            //무기 회전 처리
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        //무기도 좌우 반전 처리
        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {

        if (weaponHandler == null)
        {
            return;
        }

        //공격이 쿨다운 중이면 시간 누적
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //일정 시간마다 발사
        //공격이 입력 중이고 쿨타임이 끝났으면 공격 실행
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            //실제 공격 실행
            Attack();
        }
    }

    protected virtual void Attack()
    {
        //바라보는 방향이 있을 때만 공격
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

        //움직임 정지
        rigid.velocity = Vector2.zero;

        //모든 SpriteRenderer의 투명도 낮춰서 죽은 효과 연출
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        //모든 컴포넌트(스크립트 포함) 비활성화
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        //2초 후 오브젝트 파괴
        Destroy(gameObject, 2f);
    }
    #endregion

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        //해당 무기에서 사용할 투사체 프리팹 가져오기
        GameObject origin = monsterManager.projectilePrefabs[rangeWeaponHandler.BulletIndex];

        //지정된 위치에 투사체 생성
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);

        //투사체에 초기 정보 전달 (방향, 무기 데이터)
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }
}
