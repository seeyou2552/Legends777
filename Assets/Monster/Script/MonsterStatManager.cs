using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private int health = 10;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value,0,100);
    }

    [Range(1f, 20f)][SerializeField] private float monsterSpeed = 3f;
    public float MonsterSpeed
    {
        get => monsterSpeed;
        set => monsterSpeed = Mathf.Clamp(value, 0, 20);
    }
}
