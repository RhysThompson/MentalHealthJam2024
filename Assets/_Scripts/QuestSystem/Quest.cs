using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/Quest")]
public class Quest : Objective
{
    public List<Task> TaskData = new List<Task>();
    public List<Task> ActiveTasks { get; private set; } = new List<Task>();
    public List<Task> CompleteTasks = new List<Task>();

    public void StartTask(Task task)
    {
        if (ActiveTasks.Contains(task) || CompleteTasks.Contains(task))
        {
            Debug.Log("Attempted to start an active or complete task");
            return;
        }
        
        task.OnStart?.Invoke();
        ActiveTasks.Add(task);
        QuestHUD.Instance.UpdateQuestHUD();
    }

    public void CompleteTask(Task task)
    {
        if (CompleteTasks.Contains(task))
        {
            Debug.Log("Attempted to complete a task that was already completed:" + task.name);
            return;
        }
        if (!ActiveTasks.Contains(task))
        {
            Debug.Log("Attempted to complete a task that was not active: " + task.name);
            return;
        }
        
        task.OnComplete?.Invoke();
        ActiveTasks.Remove(task);
        CompleteTasks.Add(task);
        CheckComplete();
        QuestHUD.Instance.UpdateQuestHUD();
    }
    public void CheckComplete()
    {
        if (CompleteTasks.Count == TaskData.Count)
        {
            QuestTracker.Instance.CompleteQuest(this);
        }
        QuestHUD.Instance.UpdateQuestHUD();
    }
}
