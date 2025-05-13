using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// QuestController 초기값 num = 0 몬스터처치, num = 1 보스 처치, num = 2 방 클리어, num = 3 퍼즐 해결
// 상황에 맞게 QuestManager.Instance.QuestCheck( ); 추가

public class QuestManager : SingleTon<QuestManager>
{
    private List<QuestController> questController;

    [SerializeField] public Canvas QuestCanvas;

    public void Start()
    {
    [SerializeField] private TextMeshProUGUI questUIText0; //퀘스트UI(화면 왼쪽에 뜨는 퀘스트 정보)
    [SerializeField] private TextMeshProUGUI count0;

    [SerializeField] private TextMeshProUGUI questUIText1; 
    [SerializeField] private TextMeshProUGUI count1;

    [SerializeField] private TextMeshProUGUI questUIText2; 
    [SerializeField] private TextMeshProUGUI count2;

    

    protected override void Awake() { 
        base.Awake();
        questController = new List<QuestController>(); int i = 0;

        // QuestController 초기값 num = 0 몬스터처치, num = 1 보스 처치, num = 2 방 클리어, num = 3 퍼즐 해결
        // QuestController(int num, string text, int goal, int gold)
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Clear the room", 3, 400)); i++;
        //questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;

        QuestCanvas.gameObject.SetActive(false);
    }

    private void Update()      //퀘스트 진행사항을 퀘스트UI에 반영
    {
        if (questController == null) { return; }

        count0.text = (questController[0].PlusCount).ToString() + "/" + (questController[0].Goal).ToString(); 
        count1.text = (questController[1].PlusCount).ToString() + "/" + (questController[1].Goal).ToString();
        count2.text = (questController[2].PlusCount).ToString() + "/" + (questController[2].Goal).ToString();

        if (Input.GetKeyDown(KeyCode.M))   //테스트용 퀘스트 클리어 버튼
        {
            foreach (var quest in questController) { quest.QuestCheat(); }
        }
    }

    public void ButtonPressed() //QuestNPC에서 버튼을 눌렀을 때
    {
        QuestOn();  //퀘스트수락 상태초기화

        QuestCanvas.gameObject.SetActive(true);

        foreach (var quest in questController)
        {
            if (quest.Clear) { QuestClear(quest.Num); }  // 클리어한 퀘스트 처리
        }

        questUIText0.text = questController[0].Text; 
        questUIText1.text = questController[1].Text; 
        questUIText2.text = questController[2].Text; 

        count0.text = (questController[0].PlusCount).ToString() + "/" + (questController[0].Goal).ToString();
        count1.text = (questController[1].PlusCount).ToString() + "/" + (questController[1].Goal).ToString();
        count2.text = (questController[2].PlusCount).ToString() + "/" + (questController[2].Goal).ToString();
    }

    private void QuestOn()   //퀘스트수락 상태초기화
    {
        foreach (var quest in questController) { quest.OnOff = true; }
    }

    public void QuestCheck(int num) //퀘스트조건 체크(클리어 확인)
    {
        if (questController[num].OnOff) //퀘스트 수락 상태일 때
        { 
            questController[num].PlusCount = questController[num].PlusCount + 1;

            if (questController[num].PlusCount == questController[num].Goal)
            {
                questController[num].QuestClear(true);
            }
        }
    }

    private void QuestClear(int num) // 퀘스트를 클리어하면 NPC에 접촉후 버튼을 눌러 완료 처리
    {
        if (questController[num].Clear)
        {
            PlayerController.Instance.QuestClear(questController[num].Gold); //클리어보상 주기
            questController[num].QuestReset();        //퀘스트 객체 리셋
        }

        Debug.Log("플레이어의 골드 : " + PlayerController.Instance.Gold);
    }

    public bool QuestClearCheck()// 클리어한 퀘스트가 있는지 확인
    {
        if (questController == null) { return false; }
        foreach (var quest in questController) {  if (quest.Clear) { return true; }  }
        return false;
    }
}