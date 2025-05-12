using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;          //���� �� ������
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f;     //���� ũ��
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float speed = 1f;          //���� �ӵ�
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f;   //���� ���� ����
    public float AttackRange { get => attackRange; set => attackRange = value; }

    [Header("Knockback Info")]
    [SerializeField] private bool isOnKnockback = false;    //�˹� ���� ����
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    [SerializeField] private float knockbackPower = 0.1f;   //�˹� ��
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f;    //�˹� ���� �ð�
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public MonsterController Controller { get; private set; }  //�� ���⸦ ����ϴ� ĳ���� ��Ʈ�ѷ�

    private Animator _anim;
    private SpriteRenderer _weaponRenderer;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<MonsterController>();
        _anim = GetComponentInChildren<Animator>();
        _weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        //���� �ӵ��� ���� �ִϸ��̼� ��� �ӵ� ����
        _anim.speed = 1.0f / delay;

        //���� ũ�� ����
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimation();
    }

    public void AttackAnimation()
    {
        _anim.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        _weaponRenderer.flipY = isLeft;
    }
}
