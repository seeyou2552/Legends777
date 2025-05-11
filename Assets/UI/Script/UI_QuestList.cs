using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestList : MonoBehaviour
{
    public Transform questContainer;
    public GameObject questItemPrefab;

    public void Init(/*List<QuestData> quests*/)
    {
        // questContainer 아래에 questItemPrefab 인스턴스화하여 퀘스트 목록 표시
    }
}
