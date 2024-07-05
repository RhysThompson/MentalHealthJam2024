using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestHUDItem : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public TextMeshProUGUI taskName;

    private void Start()
    {
        questText = transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
        taskName = transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
    }
    public void ChangeText(string newQuestText, string newTaskName)
    {
        SetQuest(newQuestText);
        SetTask(newTaskName);
    }
    public void SetQuest(string newQuestText)
    {
        questText.text = "Quest: " + newQuestText;
    }
    public void SetTask(string newTaskName)
    {
        taskName.text = "Task 1: " + newTaskName;
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
