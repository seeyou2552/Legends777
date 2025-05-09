using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{

    protected Rigidbody2D _rigidbody;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAction();
    }

    private void FixedUpdate()
    {
        Movment(movementDirection);
    }

    private void Movment(Vector2 direction)
    {
        direction = direction * 5;

        _rigidbody.velocity = direction;
    }

    protected void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Entrance"))
    //    {
    //        MapController map = GameObject.Find("Map").GetComponent<MapController>();
    //        map.CreateRandomMap();

    //        //캐릭터 위치도 왼쪽 > 오른쪽 , 오른쪽 > 왼쪽 또는? 그냥 중앙으로 이동되도록 하면 좋을 것 같은데
    //        transform.position = new Vector2(0, 0);
    //    }
    //}
}
