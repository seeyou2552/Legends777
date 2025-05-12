using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int hp = 100;
    public int power = 10;
    public float speed;
    public float attackSpeed;


    private void Awake()
    {
        speed = 10;
        attackSpeed = 2f;

        // ?��???중복 방�?
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
