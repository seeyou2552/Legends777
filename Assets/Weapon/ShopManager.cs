using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public List<WeaponController> weapons;
    public List<bool> inventory;
    public List<Sprite> weaponSprites;

    private void Awake()
    {
        Instance = this;

        weapons = new List<WeaponController>(); int i = 0;
        inventory = new List<bool>();

        //num, atk, price
        weapons.Add(new WeaponController(0, 1, 0)); inventory.Add(true);   //기본무기 지급상태로 시작
        weapons.Add(new WeaponController(1, 3, 2)); inventory.Add(false); 
        weapons.Add(new WeaponController(2, 5, 4)); inventory.Add(false);
        weapons.Add(new WeaponController(3, 7, 6)); inventory.Add(false);
        weapons.Add(new WeaponController(4, 9, 8)); inventory.Add(false); 

        for (int j = 0; j < 5; j++)
        { 
            inventory[j] = false; 
        }
    }

    private void Start()
    {
        BuyOrEquipWeapon(0);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        var popup = UIManager.Instance.ShowPopup<UI_ShopPopup>("UI_ShopPopup");
        popup.Init();
    }

    private WeaponController searchWeapon(int num) 
    {   //무기를 찾아 리턴, 무기번호에 맞는 무기가 없다면 null 리턴
        foreach (WeaponController weapon in weapons) 
        {
            if (weapon.Num() == num) 
            {  
                return weapon; 
            } 
        }
        
        return null;
    }

    public void BuyOrEquipWeapon(int num) //구매 전 -> Buy() | 구매 후 -> EquipWeapon()
    {
        if (!inventory[num]) 
        {
            Debug.Log("buy or equip weapon");
            Buy(num); 
            EquipWeapon(num); 
            return; 
        }
        else 
        { 
            EquipWeapon(num); 
            return; 
        }
    }

    private void Buy(int num) //무기 구매
    {
        if(PlayerController.Instance.MinusGold(searchWeapon(num).Price()))// 플레이어 골드를 체크, 충분하면 마이너스 후 true
        {
            Debug.Log("구매: " + num);
            inventory[num] = true;  // inventory에 해당 무기 true
        }
    }

    private void EquipWeapon(int num)// 무기 장착
    {
        if (inventory[num]) {

            PlayerController.Instance.EquipWeapon(weapons[num]);
            Debug.Log(PlayerController.Instance.Equip.Num());
        }
    }
}
