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

        // ?”ì‚´ ê°œìˆ˜ +1 (ìµœë? 5)
        if (skill.arrowCount < 5)
            candidates[$"arrowCount +1 ({skill.arrowCount} -> {skill.arrowCount + 1})"] =
                () => skill.arrowCount += 1;

        // ? ë ¹ ???œì„±??(??ë²ˆë§Œ)
        if (!skill.addGhost)
            candidates["addGhost"] = () => skill.addGhost = true;

        // ??ƒ„ ì¶”ê? +1 (?„ìš”?˜ë‹¤ë©?
        if (skill.addBomb < 3) // ?ˆì‹œ ?í•œ 3
            candidates[$"addBomb +1 ({skill.addBomb} -> {skill.addBomb + 1})"] =
                () => skill.addBomb += 1;

        // ê´€???œì„±??
        if (!skill.addPenetrates)
            candidates["addPenetrates"] = () => skill.addPenetrates = true;

        // ?•ì‚° +1 (ìµœë? 5)
        if (skill.addSpread < 5)
            candidates[$"addSpread +1 ({skill.addSpread} -> {skill.addSpread + 1})"] =
                () => skill.addSpread += 1;

        // ? ë„ ?œì„±??
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

        // 4) ë²„íŠ¼ ë°°ì—´??ë¿Œë¦¬ê¸?
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
                // ?„ë³´ê°€ ë¶€ì¡±í•  ê²½ìš° ë¹?ë²„íŠ¼ ë¹„í™œ?±í™”
                btn.gameObject.SetActive(false);
            }
        }
    }
}
    