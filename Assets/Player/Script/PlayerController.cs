using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


public class PlayerController : Player
{
    public static PlayerController Instance { get; set; }

    public Rigidbody2D rigid;
    public SpriteRenderer render;
    protected AnimationHandle animationHandler;
    private Camera mainCamera;
    private int gold = 1000;
    private WeaponController weapon;
    private GameObject weaponPivot;


    void Awake()
    {
        animationHandler = GetComponent<AnimationHandle>();
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        speed = 10;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



    private void Update()

    {
        //Debug.Log(Equip.Num());
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        PlayerMove();
        if (mouseWorldPos.x < transform.position.x)
        {
            render.flipX = true;
        }
        else
        {
            render.flipX = false;
        }
    }


    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    public bool MinusGold(int gold)
    {
        if (gold <= Gold) { Gold -= gold; return true; }
        else { return false; }
    }


    public WeaponController Equip
    {
        get { return weapon; }
        private set { weapon = value; }
    }

    public void EquipWeapon(WeaponController weaponController, GameObject gameObject)
    {
        Equip = weaponController;

        if (weaponPivot != null) { Destroy(weaponPivot); }
        weaponPivot = Instantiate(gameObject, transform.position, Quaternion.identity);
    }

    public void PlayerMove()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left * speed;
            render.flipX = true;

        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right * speed;
            render.flipX = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down * speed;
        }

        rigid.velocity = new Vector2(direction.x, direction.y);
        animationHandler.Move(direction);
    }
}



