using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] private Canvas UICanvas;             //퀘스트NPC의 UI
    [SerializeField] private Button acceptButton;
    [SerializeField] private TextMeshProUGUI acceptButtonText;

    [SerializeField] private Button ExitButton;
    [SerializeField] private TextMeshProUGUI questText;

    private void Awake()
    {
        UICanvas.gameObject.SetActive(false);

        acceptButton.onClick.AddListener(() => ButtonPressed());
        ExitButton.onClick.AddListener(() => UICanvas.gameObject.SetActive(false));
        //acceptButton.text = "Accept";
    }

    private void Update()
    {
        if (QuestManager.Instance.QuestClearCheck()) { acceptButtonText.text = "Clear"; }
        else { acceptButtonText.text = "Accept"; }
    }

    private void OnCollisionEnter2D(Collision2D collision) { UICanvas.gameObject.SetActive(true); }

    private void ButtonPressed()
    {
        QuestManager.Instance.ButtonPressed(); UICanvas.gameObject.SetActive(false);
    }
}
