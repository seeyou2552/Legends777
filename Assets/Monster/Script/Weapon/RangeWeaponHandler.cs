using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : MonsterWeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;     //총알이 발사되는 위치

    [SerializeField] private int bulletIndex;                       //사용할 총알 프리팹 인덱스
    public int BulletIndex { get => bulletIndex; }

    [SerializeField] private float bulletSize = 1f;                 //총알 크기
    public float BulletSize { get => bulletSize; }

    [SerializeField] private float duration;                        //총알이 살아 있는 시간
    public float Duration { get => duration; }

    [SerializeField] private float spread;                          //총알 퍼짐 각도 범위
    public float Spread { get => spread; }

    [SerializeField] private int numberOfProjectilesPerShot;        //한 번에 발사할 수 있는 총알의 수
    public int NumberOfProjectilePerShot { get => numberOfProjectilesPerShot; }

    [SerializeField] private float multipleProjectileAngle;         //총알들 간의 고정 각도 간격
    public float MultipleProjectileAngle { get => multipleProjectileAngle; }

    [SerializeField] private Color projectileColor;                 //총알 색상 (시각적 효과)
    public Color ProjectileColor { get => projectileColor; }

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;       //총알 간 각도 간격
        int numberOfProjectilePerShot = numberOfProjectilesPerShot; //발사할 총알 수

        //총알들을 좌우 대칭으로 퍼지게 하기 위한 시작 각도 계산
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;  //기본 각도 
            float randomSpread = Random.Range(-spread, spread); //랜덤 퍼짐 적용
            angle += randomSpread;
            //실제 투사체 생성 (Controller.LookDirection: 캐릭터가 바라보는 방향)
            CreateProjectile(Controller.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 lookDirection, float angle)
    {
        Controller.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(lookDirection, angle));
    }

    //방향 벡터를 주어진 각도만큼 회전
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        //쿼터니언이 가지고 있는 회전의 수치만큼 이 Vector를 회전시켜 준다
        //교환 법칙이 되지 않아서 vectorXquaternion은 안 됨
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
