using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

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
        QuestTracker.OnQuestChanged?.Invoke(task, ObjectiveState.Started);
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
        QuestTracker.OnQuestChanged?.Invoke(task, ObjectiveState.Completed);
        QuestTracker.Instance.CheckComplete(this);
    }
    public void CheckComplete()
    {
        if (CompleteTasks.Count == TaskData.Count)
        {
            CompleteQuest();
        }
    }
    public void CompleteQuest() => QuestTracker.Instance.CompleteQuest(this);
    public void StartQuest() => QuestTracker.Instance.StartQuest(this);

    public void Start()
    {
        TaskData.ForEach(x => x.Start());
        ActiveTasks.Clear();
        CompleteTasks.Clear();
    }
}
