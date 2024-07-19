using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestHUDItem : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public List<TaskHUDItem> taskHUDItems = new List<TaskHUDItem>();
    private void Start()
    {
        questText = transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
        
    }
    public void ChangeText(string newQuestText)
    {
        SetQuest(newQuestText);
    }
    public void SetQuest(string newQuestText)
    {
        questText.text = "Quest: " + newQuestText;
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void PlayCompleteAnimation()
    {

    }

}
