using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : SingleTon<QuestManager>
{
    [SerializeField] private Button Button1;

    [SerializeField] private TextMeshProUGUI acceptButton;
    [SerializeField] private TextMeshProUGUI questText; 

    [SerializeField] private TextMeshProUGUI questUIText1;
    [SerializeField] private TextMeshProUGUI questUIText2;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI goal;


    private List<QuestController> questController;
    QuestController temp_questController; int questProgress = 0;
    int[] numbers;

    private void Start()
    {
        questController = new List<QuestController>(); int i = 0;

        // QuestController 초기화, num = 0, 몬스터 처치, num = 1, 보스 처치, num = 2, 퍼즐 기믹 해결 (아이템 획득), num = 3, 아무방 클리어
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;
        questController.Add(new QuestController(i, "Clear the room", 1, 400)); i++;

        numbers = Enumerable.Range(0, 4).OrderBy(x => Random.value).Take(2).ToArray();
        acceptButton.text = "Accept"; questText.text = questController[numbers[questProgress]].Text;

        if (!questController[numbers[questProgress]].OnOff) {
            questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);
        }

        Button1.onClick.AddListener(() => ButtonPressed());
    }

    private void Update()
    {
        count.text = (questController[numbers[questProgress]].PlusCount).ToString();

        if (questController[numbers[questProgress]].Clear) { acceptButton.text = "Clear!"; }
    }

    private void ButtonPressed()
    {
        if (!questController[numbers[questProgress]].OnOff) { 
            QuestOn();

            questUIText1.text = questController[numbers[questProgress]].Text;
            count.text = (questController[numbers[questProgress]].PlusCount).ToString();
            goal.text = (questController[numbers[questProgress]].Goal).ToString();

            questUIText1.gameObject.SetActive(true); count.gameObject.SetActive(true); goal.gameObject.SetActive(true); questUIText2.gameObject.SetActive(true);
            acceptButton.text = "Accepted";

        }
        if (questController[numbers[questProgress]].Clear) { QuestClear(); questProgress++; }
    }

    // 퀘스트 받기
    private void QuestOn()
    {
        if (questController[questProgress] != null) { 
            questController[questProgress].OnOff = true;
        }
    }

    //퀘스트 조건 체크(클리어 확인)
    public void QuestCheck(int num)
    {
        if (questController[questProgress].OnOff && questProgress == num) { questController[questProgress].PlusCount = questController[questProgress].PlusCount + 1; }
    }

    private void QuestClear()
    {
        if (questController[questProgress].Clear) { 
            PlayerController.Instance.QuestClear(questController[questProgress].Gold); //클리어 보상 주기, 퀘스트 리셋

            questController[questProgress].QuestReset();
            questText.text = questController[numbers[questProgress]].Text;

            questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);
        } 
    }


}
