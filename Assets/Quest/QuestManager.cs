using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// QuestController 초기?? num = 0 몬스??처치, num = 1 보스 처치, num = 2 ?�무�??�리?? num = 3 ?�즐 기�? ?�결
// ?�황??맞게 QuestManager.Instance.QuestCheck( ); 추�?

public class QuestManager : SingleTon<QuestManager>
{
    private List<QuestController> questController;


    [SerializeField] public Canvas QuestCanvas;
    [SerializeField] private TextMeshProUGUI questUIText1; //퀘스트UI(화면 왼쪽에 뜨는 퀘스트 정보)
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI goal;

    protected override void Awake() { base.Awake(); }

    public void Start()
    {
        questController = new List<QuestController>(); int i = 0;

        // QuestController 초기?? num = 0, 몬스??처치, num = 1, 보스 처치, num = 2, ?�즐 기�? ?�결 (?�이???�득), num = 3, ?�무�??�리??
        // QuestController(int num, string text, int goal, int gold)
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Clear the room", 1, 400)); i++;
        //questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;

        QuestCanvas.gameObject.SetActive(false);
    }

    private void Update()      //?�스??진행?�항 ?�스?�UI??반영
    {

        count.text = (questController[0].PlusCount).ToString();  //?�스??진행 ?�황 UI??반영

        if (questController == null)
        {
            return;
        }


        count.text = (questController[0].PlusCount).ToString();  //퀘스트 진행 상황 UI에 반영

        if (Input.GetKeyDown(KeyCode.M))   //?�스?�용 ?�스???�리??버튼
        {
            foreach (var quest in questController)
            {
                quest.QuestClear(true);
            }
        }
    }

    public void ButtonPressed() //QuestNPC에서 버튼을 눌렀을 때
    {
        QuestOn();  //?�스???�락?�태�?초기??

        QuestCanvas.gameObject.SetActive(true);

        foreach (var quest in questController)
        {
            if (quest.Clear) { QuestClear(quest.Num); }  // ?�리???�태????처리
        }

        questUIText1.text = questController[0].Text; goal.text = (questController[0].Goal).ToString();

    }

    private void QuestOn()   //?�스???�락?�태�?초기??
    {
        foreach (var quest in questController) { quest.OnOff = true; }
    }

    public void QuestCheck(int num) //?�스??조건 체크(?�리???�인)
    {
        if (questController[num].OnOff)
        { //?�스???�락?�태????
            questController[num].PlusCount = questController[num].PlusCount + 1;

            if (questController[num].PlusCount == questController[num].Goal)
            {
                questController[num].QuestClear(true);
            }
        }
    }

    private void QuestClear(int num) // ?�스?��? ?�리?�하�?NPC?� ?�촉???�료 처리
    {
        if (questController[num].Clear)
        {
            PlayerController.Instance.QuestClear(questController[num].Gold); //?�리??보상 주기
            questController[num].QuestReset();        //?�스??객체 리셋
        }

        Debug.Log("?�레?�어??골드 : " + PlayerController.Instance.Gold);
    }

    public bool QuestClearCheck()// 클리어한 퀘스트가 있는지 확인
    {
        if (questController == null)
        {
            return false;
        }
        foreach (var quest in questController) {  if (quest.Clear) { return true; }  }
        return false;
    }
}