using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private int num, atk, price;

    public WeaponController(int num,int atk, int price) { this.num = num; this.atk = atk; this.price = price; }

    public int Num() { return num; }

    public int Atk() {  return atk; }

    public int Price() { return price; }
}
