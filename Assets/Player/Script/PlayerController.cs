using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : SingleTon<PlayerController>
{
    private int gold = 1000;
    private WeaponController weapon;



    private void Update()
    {
        Debug.Log(Equip.Num());
    }


    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    public bool MinusGold(int gold) {
        if (gold <= Gold) { Gold -= gold; return true; }
        else { return false; } }

    //public void Equip(WeaponController weapon) { this.weapon = weapon; }

    public WeaponController Equip
    {
        get { return weapon; }
        set { weapon = value; }
    }
}
