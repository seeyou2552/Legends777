using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillOption
{
    public string Name;         // ��: "��¡ �ӵ� ����"
    public string Description;  // ��: "ȭ�� ��¡ �ӵ� +1 (5 -> 6)"
    public Sprite Icon;
    public Action ApplyAction;  // ���� ��ų ���� ����
}

public class UI_StageResult : UI_Popup
{
    [SerializeField] private SkillManager skill;

    [Header("Buttons&Labels")]

    public Button option1Button;
    public Button option2Button;
    public Button option3Button;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;

    public TextMeshProUGUI option1DescriptionText;
    public TextMeshProUGUI option2DescriptionText;
    public TextMeshProUGUI option3DescriptionText;

    public void Init()
    {
        skill = GameObject.Find("Weapon_Bow").GetComponent<SkillManager>();

        List<SkillOption> skillOptions = new List<SkillOption>();

        if (skill.shootSpeed < 50f)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "���� ���",
                Description = $"ȭ�� ��¡ �ӵ� +1 ({skill.shootSpeed} -> {skill.shootSpeed + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number0"),
                ApplyAction = () => skill.shootSpeed += 1f
            });
        }

        if (skill.arrowCount < 5)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "ȭ�� ����",
                Description = $"�߻�Ǵ� ȭ�� ���� +1 ({skill.arrowCount} -> {skill.arrowCount + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number1"),
                ApplyAction = () => skill.arrowCount += 1
            });
        }

        if (!skill.addGhost)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "�н� Ȱ",
                Description = "ĳ���Ͱ� �����ϰ� �ִ� Ȱ�� �н��� �����ȴ�.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number2"),
                ApplyAction = () => skill.addGhost = true
            });
        }

        if (skill.addBomb < 3)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "���� ȭ��",
                Description = $"������ �� �����Ǵ� ȭ�� ���� +1 ({skill.addBomb} -> {skill.addBomb + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number3"),
                ApplyAction = () => skill.addBomb += 1
            });
        }

        if (!skill.addPenetrates)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "���� ȭ��",
                Description = "���� ���� �� ���� ��մ´�.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number4"),
                ApplyAction = () => skill.addPenetrates = true
            });
        }

        if (skill.addSpread < 5)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "Į�� �п�",
                Description = $"�п��� Į�� ���� +1 ({skill.addSpread} -> {skill.addSpread + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number5"),
                ApplyAction = () => skill.addSpread += 1
            });
        }

        if (!skill.addChase)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "���� ȭ��",
                Description = "ȭ���� ���� ������ �� �ֵ��� �Ѵ�.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number6"),
                ApplyAction = () => skill.addChase = true
            });
        }

        if (!skill.addFreeze)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "���� ȭ��",
                Description = "���� ������ ����� �ñ⸦ �ο��Ѵ�.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number7"),
                ApplyAction = () => skill.addFreeze = true
            });
        }

        var rnd = new System.Random();
        var picked = new List<SkillOption>();
        int pickCount = Math.Min(3, skillOptions.Count);

        for (int i = 0; i < pickCount; i++)
        {
            int idx = rnd.Next(skillOptions.Count);
            picked.Add(skillOptions[idx]);
            skillOptions.RemoveAt(idx);
        }

        var buttons = new[] { option1Button, option2Button, option3Button };
        for (int i = 0; i < buttons.Length; i++)
        {
            var btn = buttons[i];
            btn.onClick.RemoveAllListeners();

            if (i < picked.Count)
            {
                SkillOption opt = picked[i];

                btn.gameObject.SetActive(true);

                // ��ų �̸� �� ù ��° �ؽ�Ʈ
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = opt.Name;

                // ���� �� �� ��° �ؽ�Ʈ
                btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = opt.Description;

                // ��ų ������
                btn.transform.GetChild(2).GetComponent<Image>().sprite = opt.Icon;

                GameObject popupRoot = this.gameObject;

                btn.onClick.AddListener(() =>
                {
                    opt.ApplyAction();
                    GameManager.instance.OnSkillUpgraded?.Invoke(opt.Name);
                    GameManager.instance.IsStageClear = true;
                    Destroy(popupRoot);
                });
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }


        //skill = GameObject.Find("Weapon_Bow").GetComponent<SkillManager>();
        
        //var titles = new Dictionary<string, System.Action>();
        //var descriptions = new Dictionary<string, System.Action>();
        //if (skill.shootSpeed < 50f)
        //    descriptions[$"ȭ�� ��¡ �ӵ� +1 ({skill.shootSpeed} -> {skill.shootSpeed + 1})"] =
        //        () => skill.shootSpeed += 1f;

        //if (skill.arrowCount < 5)
        //    descriptions[$"�߻�Ǵ� ȭ�� ���� +1 ({skill.arrowCount} -> {skill.arrowCount + 1})"] =
        //        () => skill.arrowCount += 1;

        //if (!skill.addGhost)
        //    descriptions["ĳ���Ͱ� �����ϰ� �ִ� Ȱ�� �н��� �����ȴ�."] = () => skill.addGhost = true;

        //if (skill.addBomb < 3)
        //    descriptions[$"������ �� �����Ǵ� ȭ�� ���� +1 ({skill.addBomb} -> {skill.addBomb + 1})"] =
        //        () => skill.addBomb += 1;

        //if (!skill.addPenetrates)
        //    descriptions["���� ���� �� ���� ��մ´�."] = () => skill.addPenetrates = true;

        //if (skill.addSpread < 5)
        //    descriptions[$"�п��� Į�� ���� +1 ({skill.addSpread} -> {skill.addSpread + 1})"] =
        //        () => skill.addSpread += 1;

        //if (!skill.addChase)
        //    descriptions["ȭ���� ���� ������ �� �ֵ��� �Ѵ�."] = () => skill.addChase = true;

        //if (!skill.addFreeze)
        //    descriptions["���� ������ ����� �ñ⸦ �ο��Ѵ�."] = () => skill.addFreeze = true;


        //var keys = new List<string>(descriptions.Keys);
        //var picked = new List<string>();
        //var rnd = new System.Random();
        //int pickCount = Math.Min(3, keys.Count);
        //for (int i = 0; i < pickCount; i++)
        //{
        //    int idx = rnd.Next(keys.Count);
        //    picked.Add(keys[idx]);
        //    keys.RemoveAt(idx);
           
        //}

        //var buttons = new[] { option1Button, option2Button, option3Button };
        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    var btn = buttons[i];
        //    btn.onClick.RemoveAllListeners();

        //    if (i < picked.Count)
        //    {
        //        string label = picked[i];
        //        Action apply = descriptions[label];

        //        btn.gameObject.SetActive(true);

        //        btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = label;
        //        //btn.GetComponentInChildren<TextMeshProUGUI>().text = label;
        //        GameObject popupRoot = this.gameObject;
        //        btn.onClick.AddListener(() =>
        //        {
        //            apply();
        //            GameManager.instance.OnSkillUpgraded?.Invoke(label);
        //            GameManager.instance.IsStageClear = true;   
        //            Destroy(popupRoot);
        //        });
        //    }
        //    else
        //    {
        //        btn.gameObject.SetActive(false);
        //    }
        //}
    }
}
    