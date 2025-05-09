using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : Player
{
    public Rigidbody2D rigid;
    public SpriteRenderer render;
    protected AnimationHandle animationHandler;
    private Camera mainCamera;
    


    void Awake()
    {
        animationHandler = GetComponent<AnimationHandle>();
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        speed = 10;
    }
    void Start()
    {
        
    }


    void Update()
    {
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
