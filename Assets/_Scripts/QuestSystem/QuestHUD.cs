using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Linq;
using UnityEditor.Experimental.GraphView;
public class QuestHUD : StaticInstance<QuestHUD>
{
    //public List<QuestHUDItem> questHUDListings;
    public GameObject questHUDItemPrefab;
    public GameObject taskHUDItemPrefab;
    public Dictionary<Quest, QuestHUDItem> questHUDItems = new Dictionary<Quest, QuestHUDItem>();
    public Dictionary<Task, TaskHUDItem> taskHUDItems = new Dictionary<Task, TaskHUDItem>();
    private void Start()
    {
        //questHUDListings = GetComponentsInChildren<QuestHUDItem>(true).ToList();
        UpdateQuestHUD();
    }
    public void UpdateQuestHUD()
    {
        foreach (QuestHUDItem item in questHUDItems.Values)
        {
            Destroy(item.gameObject);
        }
        questHUDItems.Clear();

        foreach (TaskHUDItem item in taskHUDItems.Values)
        {
            Destroy(item.gameObject);
        }
        taskHUDItems.Clear();

        GenerateQuests();
        GenerateTasks();
    }
    public void GenerateQuests()
    {
        List<Quest> quests = QuestTracker.Instance.ActiveQuests;
        foreach (Quest quest in quests)
        {
            QuestHUDItem HUDItem = Instantiate(questHUDItemPrefab, transform).GetComponent<QuestHUDItem>();
            questHUDItems[quest] = HUDItem;
            HUDItem.ChangeText(quest.name);
        }
    }

    public void GenerateTasks()
    {
        List<Task> tasks = new List<Task>(); // Create a new list to store the active tasks

        List<Quest> quests = QuestTracker.Instance.ActiveQuests;
        foreach (Quest quest in quests)
        {
            tasks.AddRange(quest.ActiveTasks); // Add all active tasks from each quest to the tasks list
        }

        foreach (Task task in tasks)
        {
            TaskHUDItem HUDItem = Instantiate(taskHUDItemPrefab, transform).GetComponent<TaskHUDItem>();
            taskHUDItems[task] = HUDItem;
            HUDItem.ChangeText("-\t" + task.name + "\n");
        }
    }
    // Update the HUD when a quest is changed
    public void UpdateQuestHUD(Objective objective, ObjectiveState state)
    {
        if(state == ObjectiveState.Completed && objective is Task)
        {
            WaitforAnimation(taskHUDItems[objective as Task]);
        }
        else
        UpdateQuestHUD();
    }
    public void OnEnable()
    {
        QuestTracker.OnQuestChanged.AddListener(UpdateQuestHUD);
    }
    public void OnDisable()
    {
        QuestTracker.OnQuestChanged.RemoveListener(UpdateQuestHUD);
    }

    public IEnumerable WaitforAnimation(TaskHUDItem task)
    {
        task.PlayCompleteAnimation();
        while (!task.isFinishedAnimation())
        {
            yield return null;
        }
        Destroy(task.gameObject);
        UpdateQuestHUD();
    }
}
