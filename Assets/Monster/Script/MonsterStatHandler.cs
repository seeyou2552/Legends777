using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatHandler : MonoBehaviour
{
    [SerializeField] private int health = 100;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    [SerializeField] private float speed = 3f;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }

    [SerializeField] private int atk = 10;
    public int Atk
    {
        get => atk;
        set => atk = value;
    }
}
