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
    
        skill = GameObject.Find("Weapon_Bow").GetComponent<SkillManager>();
        
        var candidates = new Dictionary<string, System.Action>();
        if (skill.shootSpeed < 50f)
            candidates[$"shootSpeed +1 ({skill.shootSpeed} -> {skill.shootSpeed + 1})"] =
                () => skill.shootSpeed += 1f;

        if (skill.arrowCount < 5)
            candidates[$"arrowCount +1 ({skill.arrowCount} -> {skill.arrowCount + 1})"] =
                () => skill.arrowCount += 1;

        if (!skill.addGhost)
            candidates["addGhost"] = () => skill.addGhost = true;

        if (skill.addBomb < 3) 
            candidates[$"addBomb +1 ({skill.addBomb} -> {skill.addBomb + 1})"] =
                () => skill.addBomb += 1;

        if (!skill.addPenetrates)
            candidates["addPenetrates"] = () => skill.addPenetrates = true;

        if (skill.addSpread < 5)
            candidates[$"addSpread +1 ({skill.addSpread} -> {skill.addSpread + 1})"] =
                () => skill.addSpread += 1;

        if (!skill.addChase)
            candidates["addChase"] = () => skill.addChase = true;


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
                    GameManager.instance.OnSkillUpgraded?.Invoke(label);
                    GameManager.instance.IsStageClear = true;   
                    Destroy(popupRoot);
                });
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }
}
    