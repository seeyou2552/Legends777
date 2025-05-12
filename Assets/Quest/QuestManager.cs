using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// QuestController 초기화, num = 0 몬스터 처치, num = 1 보스 처치, num = 2 아무방 클리어, num = 3 퍼즐 기믹 해결
// 상황에 맞게 QuestManager.Instance.QuestCheck( ); 추가

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
        UICanvas.gameObject.SetActive(false);
        questController = new List<QuestController>(); int i = 0;

        // QuestController 초기화, num = 0, 몬스터 처치, num = 1, 보스 처치, num = 2, 퍼즐 기믹 해결 (아이템 획득), num = 3, 아무방 클리어
        // QuestController(int num, string text, int goal, int gold)
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Clear the room", 1, 400)); i++;
        //questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;


        //numbers = Enumerable.Range(0, i).OrderBy(x => Random.value).ToArray(); //퀘스트 진행 순서 섞기
        acceptButton.text = "Accept"; questText.text = questController[numbers[questProgress]].Text;

        questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);

        Button1.onClick.AddListener(() => ButtonPressed());
        ExitButton.onClick.AddListener(()=>UICanvas.gameObject.SetActive(false));
    }

    private void Update()      //퀘스트 진행사항 퀘스트UI에 반영
    {
        //count.text = (questController[numbers[questProgress]].PlusCount).ToString();   //퀘스트 진행 상황 UI에 반영

        //if (questController[numbers[questProgress]].Clear) { acceptButton.text = "Clear!"; }




        if (Input.GetKeyDown(KeyCode.M))   //테스트용 퀘스트 클리어 버튼
        {
            foreach (var quest in questController)
            {
                quest.QuestClear(true);
            }
        }
    }

    void OnCollisionEnter(Collision collision) { UICanvas.gameObject.SetActive(true); }

    private void ButtonPressed() //버튼을 눌렀을 때
    {
        QuestOn();  //퀘스트 수락상태로 초기화

        foreach (var quest in questController) 
        {
            if (quest.Clear) { QuestClear(quest.Num); }  // 클리어 상태일 때 처리
        }

        UICanvas.gameObject.SetActive(false); //퀘스트NPC의 UI 끄기
    }


    private void QuestOn()   //퀘스트 수락상태로 초기화
    {
        foreach(var quest in questController) { quest.OnOff = true; }
    }
    
    public void QuestCheck(int num) //퀘스트 조건 체크(클리어 확인)
    {
        if (questController[num].OnOff) { //퀘스트 수락상태일 때
            questController[num].PlusCount = questController[num].PlusCount + 1;

            if(questController[num].PlusCount == questController[num].Goal)
            {
                questController[num].QuestClear(true);
            }
        }
    }

    private void QuestClear(int num) // 퀘스트를 클리어하고 NPC와 접촉해 완료 처리
    {


        if (questController[num].Clear) { 
            PlayerController.Instance.QuestClear(questController[num].Gold); //클리어 보상 주기
            questController[num].QuestReset();        //퀘스트 객체 리셋

            questText.text = questController[num].Text;    //퀘스트NPC의 UI 초기화
            questUIText1.gameObject.SetActive(false); count.gameObject.SetActive(false); goal.gameObject.SetActive(false); questUIText2.gameObject.SetActive(false);
        }

        Debug.Log("플레이어의 골드 : "+PlayerController.Instance.Gold);
    }


}
