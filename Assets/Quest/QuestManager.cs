using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// QuestController 초기화, num = 0, 몬스터 처치, num = 1, 보스 처치, num = 2, 퍼즐 기믹 해결 (아이템 획득), num = 3, 아무방 클리어
// 상황에 맞게 QuestManager.instance.QuestCheck( ); 추가

public class QuestManager : SingleTon<QuestManager>
{
    [SerializeField] private Canvas UICanvas;             //퀘스트NPC의 UI
    [SerializeField] private Button Button1;
    [SerializeField] private Button ExitButton;
    [SerializeField] private TextMeshProUGUI acceptButton;
    [SerializeField] private TextMeshProUGUI questText; 

    [SerializeField] private TextMeshProUGUI questUIText1; //퀘스트UI(화면 왼쪽에 뜨는 퀘스트 정보)
    [SerializeField] private TextMeshProUGUI questUIText2;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI goal;


    private List<QuestController> questController; 

    int questProgress = 0;   //퀘스트 진행도
    int[] numbers;  //퀘스트 진행 순서

    private void Start()
    {
        questController = new List<QuestController>(); int i = 0;

        // QuestController 초기화, num = 0, 몬스터 처치, num = 1, 보스 처치, num = 2, 퍼즐 기믹 해결 (아이템 획득), num = 3, 아무방 클리어
        // QuestController(int num, string text, int goal, int gold)
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;
        questController.Add(new QuestController(i, "Clear the room", 1, 400)); i++;

        numbers = Enumerable.Range(0, i).OrderBy(x => Random.value).ToArray(); //퀘스트 진행 순서 섞기
        acceptButton.text = "Accept"; questText.text = questController[numbers[questProgress]].Text;

        if (!questController[numbers[questProgress]].OnOff) {
            questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);
        }

        Button1.onClick.AddListener(() => ButtonPressed());
        ExitButton.onClick.AddListener(()=>UICanvas.gameObject.SetActive(false));
    }

    private void Update()
    {
        count.text = (questController[numbers[questProgress]].PlusCount).ToString();   //퀘스트 진행 상황 UI에 반영

        if (questController[numbers[questProgress]].Clear) { acceptButton.text = "Clear!"; }
    }

    private void ButtonPressed() //버튼을 눌렀을 때
    {
        if (!questController[numbers[questProgress]].OnOff) { //questProgress 순서의 퀘스트가 수락 안돼있을 때
            QuestOn();    //퀘스트 수락상태로 초기화

            questUIText1.text = questController[numbers[questProgress]].Text;             //퀘스트 UI 초기화
            count.text = (questController[numbers[questProgress]].PlusCount).ToString();
            goal.text = (questController[numbers[questProgress]].Goal).ToString();

            questUIText1.gameObject.SetActive(true); count.gameObject.SetActive(true); goal.gameObject.SetActive(true); questUIText2.gameObject.SetActive(true);
            acceptButton.text = "Accepted";
            UICanvas.gameObject.SetActive(false);  //퀘스트UI on, 퀘스트NPC의 UI off
        }


        if (questController[numbers[questProgress]].Clear) {   //클리어 상태일 때
            QuestClear();     // 클리어 시 처리
            UICanvas.gameObject.SetActive(false); //퀘스트NPC의 UI 끄기
        }
    }


    private void QuestOn()   //퀘스트 수락상태로 초기화
    {
        if (questController[numbers[questProgress]] != null) { 
            questController[numbers[questProgress]].OnOff = true;
        }
    }
    
    public void QuestCheck(int num) //퀘스트 조건 체크(클리어 확인)
    {
        if (questController[questProgress].OnOff && questProgress == num) { //퀘스트 수락상태 + 퀘스트 진행상황이 받은 인수랑 같을 때
            questController[questProgress].PlusCount = questController[questProgress].PlusCount + 1; }
    }

    private void QuestClear() // 클리어 시 처리
    {
        if (questController[numbers[questProgress]].Clear) { 
            PlayerController.Instance.QuestClear(questController[numbers[questProgress]].Gold); //클리어 보상 주기
            questController[numbers[questProgress]].QuestReset();        //퀘스트 객체 리셋

            questProgress++;   //퀘스트 진행도++
            if(questProgress >= numbers.Length) { numbers = Enumerable.Range(0, numbers.Length).OrderBy(x => Random.value).Take(2).ToArray(); questProgress = 0; }

            questText.text = questController[numbers[questProgress]].Text;    //퀘스트NPC의 UI 초기화
            questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);
        } 
    }


}
