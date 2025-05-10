using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int hp = 100;
    public int power = 100;
    public float speed { get; set;}
    public float attackSpeed { get; set;}

    private void Awake()
    {
        // ?��???중복 방�?
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
