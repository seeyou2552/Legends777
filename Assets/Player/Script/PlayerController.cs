using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


public class PlayerController : Player
{
    public event Action<int> OnGoldChanged;
    
    public static PlayerController Instance { get; set; }
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashTime;
    public bool isDashing = false;
    public float dashCoolTime = 2f; // 대쉬에 주어질 쿨타임 시간
    public float dashCool = 0f; // 대쉬의 남은 쿨타임 시간
    public GameObject dashTrailPrefab;

    public Rigidbody2D rigid;
    public SpriteRenderer render;
    protected Animator animation;
    private Camera mainCamera;
    private int gold = 1000;

    private WeaponController weapon;
    private GameObject weaponPivot;

    public SpriteRenderer weaponRenderer;


    void Awake()
    {
        animation = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        speed = 10;
        attackSpeed = 3f;

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

        if (dashCool > 0f)
        {
            dashCool -= Time.deltaTime;
        }

        PlayerMove(mouseWorldPos);
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
        private set 
        { 
            gold = value;
            OnGoldChanged?.Invoke(gold);
        }
    }

    public bool MinusGold(int gold) // 무기 구매시 골드가 충분한지 확인, 골드 감소
    {
        if (gold <= Gold) { Gold -= gold; return true; }
        else { return false; }
    }


    public WeaponController Equip // 무기 프로퍼티
    {
        get { return weapon; }
        private set { weapon = value; }
    }

    public void EquipWeapon(WeaponController weaponController)  //무기 장착, 무기 공격력 -> weaponController.Atk();
    {
        Equip = weaponController;   //무기 장착
        weaponRenderer.sprite = ShopManager.Instance.weaponSprites[Equip.Num()];
        if (Equip.Num() == 2)
        {
            weaponRenderer.material.color = Color.yellow;
        }
        else if (Equip.Num() == 3)
        {
            weaponRenderer.material.color = Color.green;
        }
        else if (Equip.Num() == 4)
        {
            weaponRenderer.material.color = Color.red;
        }
        Debug.Log(Equip.Atk());
    }

    public void QuestClear(int gold) { Gold += gold; }  //퀘스트 클리어시 골드 획득

    public void PlayerMove(Vector3 mousePos)
    {
        Vector2 direction = Vector2.zero;
        if (!isDashing)
        {
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

            if (Input.GetKeyDown(KeyCode.Space) && direction != Vector2.zero && dashCool <= 0f)
            {
                gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");
                PlayerDash(direction);
            }

            rigid.velocity = new Vector2(direction.x, direction.y);
        }
        else
        {
            Vector2 dashDir = rigid.velocity.normalized;
            rigid.velocity = dashDir * dashSpeed;
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
            CreateDashTrail(rigid.position, mousePos);
        }

        if (direction != Vector2.zero)
        {
            animation.SetBool("IsRun", true);
        }
        else
        {
            animation.SetBool("IsRun", false);
        }
    }

    void PlayerDash(Vector2 direction)
    {

        isDashing = true;
        dashTime = dashDuration;
        rigid.velocity = Vector2.zero;
        dashCool = dashCoolTime;
    }

    void CreateDashTrail(Vector2 position, Vector2 mousePos)
    {
        GameObject dashTrail = Instantiate(dashTrailPrefab, position, Quaternion.identity);

        if (mousePos.x < transform.position.x)
        {
            dashTrail.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            dashTrail.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        Destroy(dashTrail, 0.1f);
    }
}



