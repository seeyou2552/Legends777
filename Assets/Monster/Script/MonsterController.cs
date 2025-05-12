using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterController : MonoBehaviour
{
    private MonsterStatHandler monsterStatHandler;


    //?€ê²??•ì¸
    public Rigidbody2D target;
    //?€ê²Ÿì„ ì«“ì•„ê°?ìµœë? ê±°ë¦¬
    private float followRange = 15f;

    private Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


    //ì£½ìŒ ? ë¬´
    private bool isDead;

    private Vector2 knockback = Vector2.zero;   //?‰ë°± ë°©í–¥
    private float knockbackDuration = 0.0f;     //?‰ë°± ì§€???œê°„3

    protected bool isAttacking;                                 //ê³µê²© ì¤??¬ë?
    private float timeSinceLastAttack = 0;         //ë§ˆì?ë§?ê³µê²© ?´í›„ ê²½ê³¼ ?œê°„

    //ë¬´ê¸°
    [SerializeField] public Transform weaponPivot;
    [SerializeField] public MonsterWeaponHandler WeaponPrefab;         //?¥ì°©??ë¬´ê¸° ?„ë¦¬???†ìœ¼ë©??ì‹?ì„œ ì°¾ì•„???¬ìš©)
    [SerializeField] protected MonsterWeaponHandler weaponHandler;                      //?¥ì°©??ë¬´ê¸°

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
        //?Œì „ ì²˜ë¦¬
        Rotate(lookDirection);
        //ê³µê²© ?…ë ¥ ë°?ì¿¨í???ê´€ë¦?
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

        //?€ê²Ÿê¹Œì§€ ê±°ë¦¬
        float distance = DistanceToTarget();
        //?€ê²?ë°©í–¥ (ë¬´ê¸°)
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        //?Œë ˆ?´ì–´ê°€ ?¼ì • ê±°ë¦¬ ?ˆì— ?ˆì„ ?Œë§Œ ì¶”ì  ?œì‘
        if (distance <= followRange)
        {
            //ë°©í–¥
            lookDirection = direction;

            //ê³µê²© ?¬ê±°ë¦??ˆìœ¼ë¡??¤ì–´?”ì„ ê²½ìš°
            if (distance < weaponHandler.AttackRange)
            {
                isAttacking = true;
                //ê³µê²© ë²”ìœ„ ?ˆì´ë¯€ë¡??•ì?
                rigid.velocity = Vector2.zero;
                return;
            }

            //ê³µê²© ë²”ìœ„ ?ˆì´ ?„ë‹ˆê¸??Œë¬¸???´ë™
            // ?Œë ˆ?´ì–´?ê²Œ ?¥í•˜???€ì§ì„
            Vector2 nextDir = direction * monsterStatHandler.Speed * Time.fixedDeltaTime;
            // ?€ì§ì„ êµ¬í˜„ - ?ê±°ë¦¬ëŠ” ?€ì§ì´ì§€ ?Šê²Œ
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
            //?Œë ˆ?´ì–´ê°€ ì¶”ì  ë²”ìœ„ ë°–ì— ?ˆë‹¤ë©?ê°€ë§Œíˆ ?ˆì–´????
            rigid.velocity = Vector2.zero;
            animator.SetBool("isRun", false);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f; //90?„ë³´???¬ë‹¤ë©??¼ìª½??ë°”ë¼ë³´ëŠ” ê²ƒì„

        //ìºë¦­?°ê? ë³´ëŠ” ë°©í–¥?€ë¡??´ë?ì§€ ?¤ì§‘ê¸?
        spriter.flipX = isLeft;

        if (weaponPivot != null)
        {
            //weaponPivot??zì¶?ê¸°ì??¼ë¡œ rotZë§Œí¼ ?Œì „
            //ë¬´ê¸° ?Œì „ ì²˜ë¦¬
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        //ë¬´ê¸°??ì¢Œìš° ë°˜ì „ ì²˜ë¦¬
        weaponHandler?.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {

        if (weaponHandler == null)
        {
            return;
        }

        //ê³µê²©??ì¿¨ë‹¤??ì¤‘ì´ë©??œê°„ ?„ì 
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        //?¼ì • ?œê°„ë§ˆë‹¤ ë°œì‚¬
        //ê³µê²©???…ë ¥ ì¤‘ì´ê³?ì¿¨í??„ì´ ?ë‚¬?¼ë©´ ê³µê²© ?¤í–‰
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            //?¤ì œ ê³µê²© ?¤í–‰
            Attack();
        }
    }

    protected virtual void Attack()
    {
        //ë°”ë¼ë³´ëŠ” ë°©í–¥???ˆì„ ?Œë§Œ ê³µê²©
        if (lookDirection != Vector2.zero)
            weaponHandler.Attack();
    }

    #region Damage ì²˜ë¦¬
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
        //GameManager.instance.KillCount++;

        //monsterManager.RemoveActiveMonster(this);

        //?€ì§ì„ ?•ì?
        rigid.velocity = Vector2.zero;

        //ëª¨ë“  SpriteRenderer???¬ëª…????¶°??ì£½ì? ?¨ê³¼ ?°ì¶œ
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        //ëª¨ë“  ì»´í¬?ŒíŠ¸(?¤í¬ë¦½íŠ¸ ?¬í•¨) ë¹„í™œ?±í™”
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        QuestManager.Instance.QuestCheck(0);
        monsterManager.RemoveActiveMonster(this);

        //2ì´????¤ë¸Œ?íŠ¸ ?Œê´´
        Destroy(gameObject, 2f);
    }
    #endregion

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        //?´ë‹¹ ë¬´ê¸°?ì„œ ?¬ìš©???¬ì‚¬ì²??„ë¦¬??ê°€?¸ì˜¤ê¸?
        GameObject origin = monsterManager.projectilePrefabs[rangeWeaponHandler.BulletIndex];

        //ì§€?•ëœ ?„ì¹˜???¬ì‚¬ì²??ì„±
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);

        //?¬ì‚¬ì²´ì— ì´ˆê¸° ?•ë³´ ?„ë‹¬ (ë°©í–¥, ë¬´ê¸° ?°ì´??
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }
}