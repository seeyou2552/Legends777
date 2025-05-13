using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillOption
{
    public string Name;         // 예: "차징 속도 증가"
    public string Description;  // 예: "화살 차징 속도 +1 (5 -> 6)"
    public Sprite Icon;
    public Action ApplyAction;  // 실제 스킬 적용 로직
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
                Name = "빠른 사격",
                Description = $"화살 차징 속도 +1 ({skill.shootSpeed} -> {skill.shootSpeed + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number0"),
                ApplyAction = () => skill.shootSpeed += 1f
            });
        }

        if (skill.arrowCount < 5)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "화살 개수",
                Description = $"발사되는 화살 개수 +1 ({skill.arrowCount} -> {skill.arrowCount + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number1"),
                ApplyAction = () => skill.arrowCount += 1
            });
        }

        if (!skill.addGhost)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "분신 활",
                Description = "캐릭터가 보유하고 있는 활의 분신이 생성된다.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number2"),
                ApplyAction = () => skill.addGhost = true
            });
        }

        if (skill.addBomb < 3)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "지뢰 화살",
                Description = $"폭발한 후 생성되는 화살 개수 +1 ({skill.addBomb} -> {skill.addBomb + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number3"),
                ApplyAction = () => skill.addBomb += 1
            });
        }

        if (!skill.addPenetrates)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "관통 화살",
                Description = "여러 적을 한 번에 꿰뚫는다.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number4"),
                ApplyAction = () => skill.addPenetrates = true
            });
        }

        if (skill.addSpread < 5)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "칼날 분열",
                Description = $"분열될 칼날 개수 +1 ({skill.addSpread} -> {skill.addSpread + 1})",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number5"),
                ApplyAction = () => skill.addSpread += 1
            });
        }

        if (!skill.addChase)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "추적 화살",
                Description = "화살이 적을 추적할 수 있도록 한다.",
                Icon = Resources.Load<Sprite>("Sprites/SkillIcons/number6"),
                ApplyAction = () => skill.addChase = true
            });
        }

        if (!skill.addFreeze)
        {
            skillOptions.Add(new SkillOption
            {
                Name = "빙결 화살",
                Description = "적을 느리게 만드는 냉기를 부여한다.",
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

                // 스킬 이름 → 첫 번째 텍스트
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = opt.Name;

                // 설명 → 두 번째 텍스트
                btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = opt.Description;

                // 스킬 아이콘
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
        //    descriptions[$"화살 차징 속도 +1 ({skill.shootSpeed} -> {skill.shootSpeed + 1})"] =
        //        () => skill.shootSpeed += 1f;

        //if (skill.arrowCount < 5)
        //    descriptions[$"발사되는 화살 개수 +1 ({skill.arrowCount} -> {skill.arrowCount + 1})"] =
        //        () => skill.arrowCount += 1;

        //if (!skill.addGhost)
        //    descriptions["캐릭터가 보유하고 있는 활의 분신이 생성된다."] = () => skill.addGhost = true;

        //if (skill.addBomb < 3)
        //    descriptions[$"폭발한 후 생성되는 화살 개수 +1 ({skill.addBomb} -> {skill.addBomb + 1})"] =
        //        () => skill.addBomb += 1;

        //if (!skill.addPenetrates)
        //    descriptions["여러 적을 한 번에 꿰뚫는다."] = () => skill.addPenetrates = true;

        //if (skill.addSpread < 5)
        //    descriptions[$"분열될 칼날 개수 +1 ({skill.addSpread} -> {skill.addSpread + 1})"] =
        //        () => skill.addSpread += 1;

        //if (!skill.addChase)
        //    descriptions["화살이 적을 추적할 수 있도록 한다."] = () => skill.addChase = true;

        //if (!skill.addFreeze)
        //    descriptions["적을 느리게 만드는 냉기를 부여한다."] = () => skill.addFreeze = true;


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
    