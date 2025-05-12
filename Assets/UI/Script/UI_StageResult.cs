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
            candidates[$"화살 속도 +1 ({skill.shootSpeed} → {skill.shootSpeed + 1})"] =
                () => skill.shootSpeed += 1f;

        // 화살 개수 +1 (최대 5)
        if (skill.arrowCount < 5)
            candidates[$"화살 개수 +1 ({skill.arrowCount} → {skill.arrowCount + 1})"] =
                () => skill.arrowCount += 1;

        // 유령 활 활성화 (한 번만)
        if (!skill.addGhost)
            candidates["유령 활 활성화"] = () => skill.addGhost = true;

        // 폭탄 추가 +1 (필요하다면)
        if (skill.addBomb < 3) // 예시 상한 3
            candidates[$"폭탄 +1 ({skill.addBomb} → {skill.addBomb + 1})"] =
                () => skill.addBomb += 1;

        // 불 화살 활성화
        if (!skill.addBurn)
            candidates["불 화살 활성화"] = () => skill.addBurn = true;

        // 관통 활성화
        if (!skill.addPenetrates)
            candidates["관통 화살 활성화"] = () => skill.addPenetrates = true;

        // 확산 +1 (최대 5)
        if (skill.addSpread < 5)
            candidates[$"확산 +1 ({skill.addSpread} → {skill.addSpread + 1})"] =
                () => skill.addSpread += 1;

        // 유도 활성화
        if (!skill.addChase)
            candidates["유도 화살 활성화"] = () => skill.addChase = true;


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

        // 4) 버튼 배열에 뿌리기
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
                // 후보가 부족할 경우 빈 버튼 비활성화
                btn.gameObject.SetActive(false);
            }
        }
    }
}
    