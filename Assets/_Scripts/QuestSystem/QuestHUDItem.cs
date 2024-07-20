using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestHUDItem : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public Quest quest;
    public Dictionary<Task, TaskHUDItem> taskHUDItems = new Dictionary<Task, TaskHUDItem>();
    public GameObject taskHUDItemPrefab;
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

    public void GenerateTasks()
    {
        foreach (TaskHUDItem item in taskHUDItems.Values)
        {
            Destroy(item.gameObject);
        }
        taskHUDItems.Clear();

        foreach (Task task in quest.ActiveTasks)
        {
            TaskHUDItem HUDItem = Instantiate(taskHUDItemPrefab, transform).GetComponent<TaskHUDItem>();
            HUDItem.task = task;
            taskHUDItems[task] = HUDItem;
            HUDItem.ChangeText("-\t" + task.name + "\n");
        }
    }


}
