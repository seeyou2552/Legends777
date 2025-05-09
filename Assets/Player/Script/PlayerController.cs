using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerController : SingleTon<PlayerController>
{
    private int gold = 1000;
    private WeaponController weapon;
    private GameObject weaponPivot;


    private void Update()
    {
        //Debug.Log(Equip.Num());
    }


    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    public bool MinusGold(int gold) {
        if (gold <= Gold) { Gold -= gold; return true; }
        else { return false; } }

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
}
