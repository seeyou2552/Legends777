using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : MonsterWeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;     //�Ѿ��� �߻�Ǵ� ��ġ

    [SerializeField] private int bulletIndex;                       //����� �Ѿ� ������ �ε���
    public int BulletIndex { get => bulletIndex; }

    [SerializeField] private float bulletSize = 1f;                 //�Ѿ� ũ��
    public float BulletSize { get => bulletSize; }

    [SerializeField] private float duration;                        //�Ѿ��� ��� �ִ� �ð�
    public float Duration { get => duration; }

    [SerializeField] private float spread;                          //�Ѿ� ���� ���� ����
    public float Spread { get => spread; }

    [SerializeField] private int numberOfProjectilesPerShot;        //�� ���� �߻��� �� �ִ� �Ѿ��� ��
    public int NumberOfProjectilePerShot { get => numberOfProjectilesPerShot; }

    [SerializeField] private float multipleProjectileAngle;         //�Ѿ˵� ���� ���� ���� ����
    public float MultipleProjectileAngle { get => multipleProjectileAngle; }

    [SerializeField] private Color projectileColor;                 //�Ѿ� ���� (�ð��� ȿ��)
    public Color ProjectileColor { get => projectileColor; }

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;       //�Ѿ� �� ���� ����
        int numberOfProjectilePerShot = numberOfProjectilesPerShot; //�߻��� �Ѿ� ��

        //�Ѿ˵��� �¿� ��Ī���� ������ �ϱ� ���� ���� ���� ���
        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;  //�⺻ ���� 
            float randomSpread = Random.Range(-spread, spread); //���� ���� ����
            angle += randomSpread;
            //���� ����ü ���� (Controller.LookDirection: ĳ���Ͱ� �ٶ󺸴� ����)
            CreateProjectile(Controller.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 lookDirection, float angle)
    {
        Controller.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(lookDirection, angle));
    }

    //���� ���͸� �־��� ������ŭ ȸ��
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        //���ʹϾ��� ������ �ִ� ȸ���� ��ġ��ŭ �� Vector�� ȸ������ �ش�
        //��ȯ ��Ģ�� ���� �ʾƼ� vectorXquaternion�� �� ��
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
