using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopPopup : UI_Popup
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;
    [SerializeField] private Button exitButton;

    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI button2Text;
    [SerializeField] private TextMeshProUGUI button3Text;
    [SerializeField] private TextMeshProUGUI button4Text;
    [SerializeField] private TextMeshProUGUI button5Text;

    private ShopManager shopManager;

    public void Init()
    {
        shopManager = ShopManager.Instance;

        button1.onClick.AddListener(() => ButtonPressed(0));
        button2.onClick.AddListener(() => ButtonPressed(1));
        button3.onClick.AddListener(() => ButtonPressed(2));
        button4.onClick.AddListener(() => ButtonPressed(3));
        button5.onClick.AddListener(() => ButtonPressed(4));

        exitButton.onClick.AddListener(() => UIManager.Instance.ClosePopupUI(this));

        Refresh();
    }

    private void ButtonPressed(int num) //구매 전 -> Buy() | 구매 후 -> EquipWeapon()
    {
        shopManager.BuyOrEquipWeapon(num);
        Refresh();
    }

    public void Refresh()
    {
        if (shopManager.inventory[0]) { button1Text.text = "장착하기"; }
        if (shopManager.inventory[1]) { button2Text.text = "장착하기"; }   //무기를 구매 했는지 확인, 했으면 버튼 글자 바꿈
        if (shopManager.inventory[2]) { button3Text.text = "장착하기"; }
        if (shopManager.inventory[3]) { button4Text.text = "장착하기"; }
        if (shopManager.inventory[4]) { button5Text.text = "장착하기"; }

        switch (PlayerController.Instance.Equip.Num())  //무기를 장착했는지 확인
        {
            case 0:
                button1Text.text = "장착 중";
                break;
            case 1:
                button2Text.text = "장착 중";
                break;
            case 2:
                button3Text.text = "장착 중";
                break;
            case 3:
                button4Text.text = "장착 중";
                break;
            case 4:
                button5Text.text = "장착 중";
                break;
        }
    }
}
