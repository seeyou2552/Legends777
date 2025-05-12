using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    private int num;
    private bool onOff;
    private bool clear = false;
    private string text;
    private int count;
    private int goal;
    private int gold;

    public QuestController(int num, string text, int goal, int gold)
    {
        this.num = num; this.onOff = false; this.clear = false; this.text = text; this.count = 0; this.goal = goal; this.gold = gold;
    }

    public int Num
    {
        get { return num; }
        private set { num = value; }
    }

    public string Text
    {
        get { return text; }
    }
    public bool OnOff
    {
        get { return onOff; }
        set { onOff = value; }
    }

    public int PlusCount
    {
        get { return count; }
        set { count = value; if (count >= goal) { clear = true; } }
    }

    public int Gold { get { return gold; } }

    public int Goal { get { return goal; } }

    public bool Clear { get { return clear; } }

    public void QuestReset() { this.onOff = false; this.clear = false; this.count = 0; }
}
