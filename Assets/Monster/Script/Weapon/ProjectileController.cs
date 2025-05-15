using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private RangeWeaponHandler rangeWeaponHandler;          //�߻翡 ���� ���� ���� ����

    private float currentDuration;      //������� ��� �ִ� �ð�
    private Vector2 direction;          //�߻� ����
    private bool isReady;               //�߻� �غ� �Ϸ� ����
    private Transform pivot;            //�Ѿ��� �ð� ȸ���� ���� �ǹ�

    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;

    public MonsterController monsterController;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        //�ǹ� ������Ʈ(��������Ʈ ȸ����)
        pivot = transform.GetChild(0);
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, MonsterController monsterController)
    {
        this.monsterController = monsterController;
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;

        //ũ�� �� ���� ����
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        _sprite.color = weaponHandler.ProjectileColor;

        //right�� ������ �� ������Ʈ�� transform�� �������� �ٶ󺸴� ������ �������� �ǰ�, �����ϰ� �Ǹ� �������� ȸ���� �ڵ����� �̷������ �ȴ�.
        //ȸ�� ���� ���� (���� ���� ����)
        transform.right = this.direction;

        //X ���⿡ ���� ��������Ʈ ���Ʒ� ����
        if (direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    private void Update()
    {
        if (!isReady)
            return;

        //���� �ð� ����
        currentDuration += Time.deltaTime;

        //������ ���� �ð� �ʰ� �� �ڵ� �ı�
        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position);
        }

        //���� �̵� ó�� (����, �ӵ�)
        _rigid.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� �浹 �� -> �ణ �� ��ġ�� ����Ʈ �����ϰ� �ı�
        if (collision.CompareTag("Wall"))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f);
        }
        //�÷��̾� ���̾ �浹���� ���
        else if (collision.CompareTag("Player"))
        {
            //������ ������ ���� ü�� �ý����� �ִ��� Ȯ��
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                //�÷��̾�� ������ ����
                //player.gameObject.GetComponent<Player>().hp -= monsterController.gameObject.GetComponent<MonsterStatHandler>().Atk;
                int dmg = monsterController.GetComponent<MonsterStatHandler>().Atk;
                PlayerManager.Instance.ApplyDamage(dmg);
            }

            DestroyProjectile(collision.ClosestPoint(transform.position));
        }
    }

    //����ü �ı� �Լ�
    private void DestroyProjectile(Vector3 position)
    {
        Destroy(this.gameObject);
    }
}
