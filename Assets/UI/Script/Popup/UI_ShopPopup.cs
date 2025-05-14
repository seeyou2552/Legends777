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

    private void ButtonPressed(int num) //���� �� -> Buy() | ���� �� -> EquipWeapon()
    {
        shopManager.BuyOrEquipWeapon(num);
        Refresh();
    }

    public void Refresh()
    {
        if (shopManager.inventory[0]) { button1Text.text = "�����ϱ�"; }
        if (shopManager.inventory[1]) { button2Text.text = "�����ϱ�"; }   //���⸦ ���� �ߴ��� Ȯ��, ������ ��ư ���� �ٲ�
        if (shopManager.inventory[2]) { button3Text.text = "�����ϱ�"; }
        if (shopManager.inventory[3]) { button4Text.text = "�����ϱ�"; }
        if (shopManager.inventory[4]) { button5Text.text = "�����ϱ�"; }

        switch (PlayerController.Instance.Equip.Num())  //���⸦ �����ߴ��� Ȯ��
        {
            case 0:
                button1Text.text = "���� ��";
                break;
            case 1:
                button2Text.text = "���� ��";
                break;
            case 2:
                button3Text.text = "���� ��";
                break;
            case 3:
                button4Text.text = "���� ��";
                break;
            case 4:
                button5Text.text = "���� ��";
                break;
        }
    }
}
