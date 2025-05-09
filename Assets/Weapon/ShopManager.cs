using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button Button1;
    [SerializeField] private Button Button2;
    [SerializeField] private Button Button3;

    [SerializeField] private TextMeshProUGUI button11;
    [SerializeField] private TextMeshProUGUI button22;
    [SerializeField] private TextMeshProUGUI button33;


    [SerializeField] Button BaseWeaponButton;


    List<WeaponController> weapons;
    List<bool> inventory;


    private void Awake()
    {
        weapons = new List<WeaponController>(); int i = 0;
        inventory = new List<bool>();

        //num, atk, price
        weapons.Add(new WeaponController(i, 1, 0)); inventory.Add(true); i++;   //기본무기 지급상태로 시작
        weapons.Add(new WeaponController(i, 3, 2)); inventory.Add(false); i++;
        weapons.Add(new WeaponController(i, 5, 4)); inventory.Add(false); i++;
        weapons.Add(new WeaponController(i, 7, 6)); inventory.Add(false); i++;



        Button1.onClick.AddListener(()=>ButtonPressed(1));
        Button2.onClick.AddListener(() => ButtonPressed(2));
        Button3.onClick.AddListener(() => ButtonPressed(3));

        BaseWeaponButton.onClick.AddListener(() => ButtonPressed(0));
        ButtonPressed(0);
    }

    private void Update()
    {
        if (inventory[1]) { button11.text = "Equip"; }
        if (inventory[2]) { button22.text = "Equip"; }
        if (inventory[3]) { button33.text = "Equip"; }

        switch (PlayerController.Instance.Equip.Num())
        {
            case 1:
                button11.text = "Equipped";
                break;
            case 2:
                button22.text = "Equipped";
                break;
            case 3:
                button33.text = "Equipped";
                break;
        }
    }


    private WeaponController searchWeapon(int num) {   //무기를 찾아 리턴, 무기번호에 맞는 무기가 없다면 null 리턴
        foreach (WeaponController weapon in weapons) {
            if (weapon.Num() == num) {  return weapon; } }
        
        return null;
    }

    private void ButtonPressed(int num) //구매 전 -> Buy() | 구매 후 -> EquipWeapon()
    {
        if (!inventory[num]) { Buy(num); return; }
        else { EquipWeapon(num); return; }
    }

    private void Buy(int num)
    {
        if(PlayerController.Instance.MinusGold(searchWeapon(num).Price()))// 플레이어 골드를 체크, 충분하면 마이너스 후 true
        {
            // inventory에 해당 무기 true
            inventory[num] = true;
        }
    }


    private void EquipWeapon(int num)
    {
        if (inventory[num]) {
            PlayerController.Instance.Equip = weapons[num]; 

            //weapon prefab 부착
        
        }
    }
}
