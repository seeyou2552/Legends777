using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// QuestController ì´ˆê¸°?? num = 0 ëª¬ìŠ¤??ì²˜ì¹˜, num = 1 ë³´ìŠ¤ ì²˜ì¹˜, num = 2 ?„ë¬´ë°??´ë¦¬?? num = 3 ?¼ì¦ ê¸°ë? ?´ê²°
// ?í™©??ë§ê²Œ QuestManager.Instance.QuestCheck( ); ì¶”ê?

public class QuestManager : SingleTon<QuestManager>
{
    private List<QuestController> questController;

    [SerializeField] private Canvas UICanvas;
    [SerializeField] private TextMeshProUGUI questUIText1; //?˜ìŠ¤?¸UI(?”ë©´ ?¼ìª½???¨ëŠ” ?˜ìŠ¤???•ë³´)
    [SerializeField] private TextMeshProUGUI questUIText2;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI goal;

    protected override void Awake() { base.Awake(); }

    public void Init()
    {
        questController = new List<QuestController>(); int i = 0;

        // QuestController ì´ˆê¸°?? num = 0, ëª¬ìŠ¤??ì²˜ì¹˜, num = 1, ë³´ìŠ¤ ì²˜ì¹˜, num = 2, ?¼ì¦ ê¸°ë? ?´ê²° (?„ì´???ë“), num = 3, ?„ë¬´ë°??´ë¦¬??
        // QuestController(int num, string text, int goal, int gold)
        questController.Add(new QuestController(i, "Kill the monster", 1, 100)); i++;
        questController.Add(new QuestController(i, "Kill the boss", 1, 200)); i++;
        questController.Add(new QuestController(i, "Clear the room", 1, 400)); i++;
        //questController.Add(new QuestController(i, "Solve the puzzle", 1, 300)); i++;
    }

    private void Update()      //?˜ìŠ¤??ì§„í–‰?¬í•­ ?˜ìŠ¤?¸UI??ë°˜ì˜
    {
        //count.text = (questController[0].PlusCount).ToString();  //?˜ìŠ¤??ì§„í–‰ ?í™© UI??ë°˜ì˜


        if (Input.GetKeyDown(KeyCode.M))   //?ŒìŠ¤?¸ìš© ?˜ìŠ¤???´ë¦¬??ë²„íŠ¼
        {
            foreach (var quest in questController)
            {
                quest.QuestClear(true);
            }
        }
    }

    public void ButtonPressed() //ë²„íŠ¼???Œë?????
    {
        QuestOn();  //?˜ìŠ¤???˜ë½?íƒœë¡?ì´ˆê¸°??

        UICanvas.gameObject.SetActive(true);

        foreach (var quest in questController)
        {
            if (quest.Clear) { QuestClear(quest.Num); }  // ?´ë¦¬???íƒœ????ì²˜ë¦¬
        }

        questUIText1.text = questController[0].Text; goal.text = (questController[0].Goal).ToString();

    }

    private void QuestOn()   //?˜ìŠ¤???˜ë½?íƒœë¡?ì´ˆê¸°??
    {
        foreach (var quest in questController) { quest.OnOff = true; }
    }

    public void QuestCheck(int num) //?˜ìŠ¤??ì¡°ê±´ ì²´í¬(?´ë¦¬???•ì¸)
    {
        if (questController[num].OnOff)
        { //?˜ìŠ¤???˜ë½?íƒœ????
            questController[num].PlusCount = questController[num].PlusCount + 1;

            if (questController[num].PlusCount == questController[num].Goal)
            {
                questController[num].QuestClear(true);
            }
        }
    }

    private void QuestClear(int num) // ?˜ìŠ¤?¸ë? ?´ë¦¬?´í•˜ê³?NPC?€ ?‘ì´‰???„ë£Œ ì²˜ë¦¬
    {
        if (questController[num].Clear)
        {
            PlayerController.Instance.QuestClear(questController[num].Gold); //?´ë¦¬??ë³´ìƒ ì£¼ê¸°
            questController[num].QuestReset();        //?˜ìŠ¤??ê°ì²´ ë¦¬ì…‹
        }

        Debug.Log("?Œë ˆ?´ì–´??ê³¨ë“œ : " + PlayerController.Instance.Gold);
    }

    public bool QuestClearCheck()
    {
        foreach (var quest in questController) { if (quest.Clear) { return true; } }
        return false;
    }
}