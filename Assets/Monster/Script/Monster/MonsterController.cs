using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    private MonsterStatHandler monsterStatHandler;


    //?占쏙옙??占쎌씤
    public Rigidbody2D target;
    //?占쎄쿊??已볦븘占?理쒙옙? 嫄곕━
    private float followRange = 15f;

    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


    //二쎌쓬 ?占쎈Т
    private bool isDead;

    private Vector2 knockback = Vector2.zero;   //?占쎈갚 諛⑺뼢
    private float knockbackDuration = 0.0f;     //?占쎈갚 吏???占쎄컙3

    protected bool isAttacking;                                 //怨듦꺽 占??占쏙옙?
    private float timeSinceLastAttack = 0;         //留덌옙?占?怨듦꺽 ?占쏀썑 寃쎄낵 ?占쎄컙

    //臾닿린
    [SerializeField] public Transform weaponPivot;
    [SerializeField] public MonsterWeaponHandler WeaponPrefab;         //?占쎌갑??臾닿린 ?占쎈━???占쎌쑝占??占쎌떇?占쎌꽌 李얠븘???占쎌슜)
    [SerializeField] protected MonsterWeaponHandler weaponHandler;                      //?占쎌갑??臾닿린

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
        //?占쎌쟾 泥섎━
        Rotate(lookDirection);
        //怨듦꺽 ?占쎈젰 占?荑⑨옙???愿占?
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

        //?占쎄쿊源뚯? 嫄곕━
        float distance = DistanceToTarget();
        //?占쏙옙?諛⑺뼢 (臾닿린)
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        //?占쎈젅?占쎌뼱媛 ?占쎌젙 嫄곕━ ?占쎌뿉 ?占쎌쓣 ?占쎈쭔 異붿쟻 ?占쎌옉
        if (distance <= followRange)
        {
            //諛⑺뼢
            lookDirection = direction;

            //怨듦꺽 ?占쎄굅占??占쎌쑝占??占쎌뼱?占쎌쓣 寃쎌슦
            if (distance < weaponHandler.AttackRange)
            {
                isAttacking = true;
                //怨듦꺽 踰붿쐞 ?占쎌씠誘占??占쏙옙?
                rigid.velocity = Vector2.zero;
                return;
            }

            //怨듦꺽 踰붿쐞 ?占쎌씠 ?占쎈땲占??占쎈Ц???占쎈룞
            // ?占쎈젅?占쎌뼱?占쎄쾶 ?占쏀븯???占쎌쭅??
            Vector2 nextDir = direction * monsterStatHandler.Speed * Time.fixedDeltaTime;

            rigid.MovePosition(rigid.position + nextDir);
            animator.SetBool("IsRun", true);
        }
        else
        {
            //?占쎈젅?占쎌뼱媛 異붿쟻 踰붿쐞 諛뽰뿉 ?占쎈떎占?媛留뚰엳 ?占쎌뼱????
            rigid.velocity = Vector2.zero;
            animator.SetBool("IsRun", false);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; //90?占쎈낫???占쎈떎占??占쎌そ??諛붾씪蹂대뒗 寃껋엫

        //罹먮┃?占쏙옙? 蹂대뒗 諛⑺뼢?占쏙옙??占쏙옙?吏 ?占쎌쭛占?
        spriter.flipX = isLeft;

        if (weaponPivot != null)
        {
            //weaponPivot??z占?湲곤옙??占쎈줈 rotZ留뚰겮 ?占쎌쟾
            //臾닿린 ?占쎌쟾 泥섎━
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        //臾닿린??醫뚯슦 諛섏쟾 泥섎━
        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {

        if (weaponHandler == null)
        {
            return;
        }

        //怨듦꺽??荑⑤떎??以묒씠占??占쎄컙 ?占쎌쟻
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //?占쎌젙 ?占쎄컙留덈떎 諛쒖궗
        //怨듦꺽???占쎈젰 以묒씠占?荑⑨옙??占쎌씠 ?占쎈궗?占쎈㈃ 怨듦꺽 ?占쏀뻾
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            //?占쎌젣 怨듦꺽 ?占쏀뻾
            Attack();
        }
    }

    protected virtual void Attack()
    {
        //諛붾씪蹂대뒗 諛⑺뼢???占쎌쓣 ?占쎈쭔 怨듦꺽
        if (lookDirection != Vector2.zero)
            weaponHandler.Attack();
    }

    #region Damage 泥섎━
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

        //?占쎌쭅???占쏙옙?
        rigid.velocity = Vector2.zero;

        //紐⑤뱺 SpriteRenderer???占쎈챸????占쏙옙??二쏙옙? ?占쎄낵 ?占쎌텧
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        //紐⑤뱺 而댄룷?占쏀듃(?占쏀겕由쏀듃 ?占쏀븿) 鍮꾪솢?占쏀솕
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        QuestManager.Instance.QuestCheck(0);
        monsterManager.RemoveActiveMonster(this);

        //2占????占쎈툕?占쏀듃 ?占쎄눼
        Destroy(gameObject, 2f);
    }
    #endregion

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        //?占쎈떦 臾닿린?占쎌꽌 ?占쎌슜???占쎌궗占??占쎈━??媛?占쎌삤占?
        GameObject origin = monsterManager.projectilePrefabs[rangeWeaponHandler.BulletIndex];

        //吏?占쎈맂 ?占쎌튂???占쎌궗占??占쎌꽦
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);

        //?占쎌궗泥댁뿉 珥덇린 ?占쎈낫 ?占쎈떖 (諛⑺뼢, 臾닿린 ?占쎌씠??
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }
}