using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private RangeWeaponHandler rangeWeaponHandler;          //발사에 사용된 무기 정보 참조

    private float currentDuration;      //현재까지 살아 있는 시간
    private Vector2 direction;          //발사 방향
    private bool isReady;               //발사 준비 완료 여부
    private Transform pivot;            //총알의 시각 회전을 위한 피벗

    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;

    public MonsterController monsterController;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        //피벗 오브젝트(스프라이트 회전용)
        pivot = transform.GetChild(0);
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, MonsterController monsterController)
    {
        this.monsterController = monsterController;
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;

        //크기 및 색상 적용
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        _sprite.color = weaponHandler.ProjectileColor;

        //right는 실제로 이 오브젝트의 transform의 오른쪽을 바라보는 방향을 가져오게 되고, 설정하게 되면 나머지의 회전이 자동으로 이루어지게 된다.
        //회전 방향 설정 (정면 방향 지정)
        transform.right = this.direction;

        //X 방향에 따라 스프라이트 위아래 반전
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

        //생존 시간 누적
        currentDuration += Time.deltaTime;

        //설정된 지속 시간 초과 시 자동 파괴
        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position);
        }

        //물리 이동 처리 (방향, 속도)
        _rigid.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //지형 충돌 시 -> 약간 앞 위치에 이펙트 생성하고 파괴
        if (collision.CompareTag("Wall"))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f);
        }
        //플레이어 레이어에 충돌했을 경우
        else if (collision.CompareTag("Player"))
        {
            //데미지 적용을 위해 체력 시스템이 있는지 확인
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                //플레이어에게 데미지 적용
                //player.gameObject.GetComponent<Player>().hp -= monsterController.gameObject.GetComponent<MonsterStatHandler>().Atk;
                int dmg = monsterController.GetComponent<MonsterStatHandler>().Atk;
                PlayerManager.Instance.ApplyDamage(dmg);
            }

            DestroyProjectile(collision.ClosestPoint(transform.position));
        }
    }

    //투사체 파괴 함수
    private void DestroyProjectile(Vector3 position)
    {
        Destroy(this.gameObject);
    }
}
