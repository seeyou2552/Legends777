using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageResult : UI_Popup
{
    [SerializeField] private SkillManager skill;

    [Header("Buttons&Labels")]

    public Button option1Button;
    public Button option2Button;
    public Button option3Button;


    public void Init()
    {
        if(skill == null)
        {
            skill = GameObject.Find("Weapon_Bow").GetComponent<SkillManager>();
        }
        var candidates = new Dictionary<string, System.Action>();
        if (skill.shootSpeed < 50f)
            candidates[$"ȭ�� �ӵ� +1 ({skill.shootSpeed} �� {skill.shootSpeed + 1})"] =
                () => skill.shootSpeed += 1f;

        // ȭ�� ���� +1 (�ִ� 5)
        if (skill.arrowCount < 5)
            candidates[$"ȭ�� ���� +1 ({skill.arrowCount} �� {skill.arrowCount + 1})"] =
                () => skill.arrowCount += 1;

        // ���� Ȱ Ȱ��ȭ (�� ����)
        if (!skill.addGhost)
            candidates["���� Ȱ Ȱ��ȭ"] = () => skill.addGhost = true;

        // ��ź �߰� +1 (�ʿ��ϴٸ�)
        if (skill.addBomb < 3) // ���� ���� 3
            candidates[$"��ź +1 ({skill.addBomb} �� {skill.addBomb + 1})"] =
                () => skill.addBomb += 1;

        // �� ȭ�� Ȱ��ȭ
        if (!skill.addBurn)
            candidates["�� ȭ�� Ȱ��ȭ"] = () => skill.addBurn = true;

        // ���� Ȱ��ȭ
        if (!skill.addPenetrates)
            candidates["���� ȭ�� Ȱ��ȭ"] = () => skill.addPenetrates = true;

        // Ȯ�� +1 (�ִ� 5)
        if (skill.addSpread < 5)
            candidates[$"Ȯ�� +1 ({skill.addSpread} �� {skill.addSpread + 1})"] =
                () => skill.addSpread += 1;

        // ���� Ȱ��ȭ
        if (!skill.addChase)
            candidates["���� ȭ�� Ȱ��ȭ"] = () => skill.addChase = true;


        var keys = new List<string>(candidates.Keys);
        var picked = new List<string>();
        var rnd = new System.Random();
        int pickCount = Math.Min(3, keys.Count);
        for (int i = 0; i < pickCount; i++)
        {
            int idx = rnd.Next(keys.Count);
            picked.Add(keys[idx]);
            keys.RemoveAt(idx);
        }

        // 4) ��ư �迭�� �Ѹ���
        var buttons = new[] { option1Button, option2Button, option3Button };
        for (int i = 0; i < buttons.Length; i++)
        {
            var btn = buttons[i];
            btn.onClick.RemoveAllListeners();

            if (i < picked.Count)
            {
                string label = picked[i];
                Action apply = candidates[label];

                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = label;
                GameObject popupRoot = this.gameObject;
                btn.onClick.AddListener(() =>
                {
                    apply();
                    GameManager.instance.IsStageClear = true;   
                    Destroy(popupRoot);
                });
            }
            else
            {
                // �ĺ��� ������ ��� �� ��ư ��Ȱ��ȭ
                btn.gameObject.SetActive(false);
            }
        }
    }
}
    
