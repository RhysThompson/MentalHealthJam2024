using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.ComponentModel;
using System.Collections;

[System.Serializable]
public enum ProgressState
{
    Inactive,
    Active,
    Complete
}

[Serializable]
public abstract class Objective
{
    //public int id;
    public string name;
    //public string description;
    public ProgressState state = ProgressState.Inactive;
    //public UnityEvent OnComplete = new UnityEvent();
    //public UnityEvent OnStart = new UnityEvent();
}

[Serializable]
public class Quest : Objective
{
    public List<Task> tasks = new List<Task>();
    public bool hasAllCompleteTasks()
    {
        return tasks.All(x => x.state == ProgressState.Complete);
    }

    public List<Task> GetActiveTasks()
    {
        return tasks.Where(x => x.state == ProgressState.Active).ToList();
    }
    public List<Task> GetCompletedTasks()
    {
        return tasks.Where(x => x.state == ProgressState.Complete).ToList();
    }
}

[Serializable]
public class Task : Objective
{ }



[System.Serializable]
public class QuestTracker : Singleton<QuestTracker>
{
    public List<Quest> Quests = new List<Quest>();
    //public static UnityEvent<Objective, ProgressState> OnQuestChanged = new UnityEvent<Objective, ProgressState>();
    public List<GameObject> QuestListeners = new List<GameObject>();
    public void Start()
    {
        // Trigger the OnQuestChanged event for all quests
        foreach(Quest quest in Quests)
        {
            SendToListeners(quest);
        }
    }
    
    public void SetObjectiveState(Objective objective, ProgressState state)
    {
        objective.state = state;
        SendToListeners(objective);
    }
    public void SetQuestState(string questName, ProgressState state)
    { SetObjectiveState(StringToQuest(questName), state); }

    public void SetTaskState(string taskName, ProgressState state)
    { SetObjectiveState(StringToTask(taskName), state); }

    public Quest StringToQuest(string questName)
    {
        foreach (Quest quest in Quests)
        {
            if (quest.name == questName)
            {
                return quest;
            }
        }

        print(questName + " not found in Quests");
        return null;
    }
    public Task StringToTask(string taskName)
    {
        foreach (Quest quest in Quests)
        {
            foreach (Task task in quest.tasks)
            {
                if (task.name == taskName)
                {
                    return task;
                }
            }
        }

        print(taskName + " not found in Tasks");
        return null;
    }

    public List<Quest> GetQuestsWithState(ProgressState state)
    {
        return Quests.Where(x => x.state == state).ToList();
    }
    public List<Quest> GetActiveQuests() => GetQuestsWithState(ProgressState.Active);
    public List<Quest> GetCompleteQuests() => GetQuestsWithState(ProgressState.Complete);

    public void AddListener(GameObject newListener)
    {
       QuestListeners.Add(newListener);
    }
    public void RemoveListener(GameObject listener)
    {
        QuestListeners.Remove(listener);
    }
    public void SendToListeners(Objective updatedObjective)
    {
        foreach (GameObject gameObject in QuestListeners)
            gameObject.SendMessage("OnQuestUpdated", updatedObjective);
    }

    public Quest GetQuestFromTask(Task task)
    {
        foreach (Quest quest in Quests)
        {
            foreach (Task t in quest.tasks)
            {
                if (t == task)
                {
                    return quest;
                }
            }
        }
        return null;
    }

    //the following 4 methods are used for attaching to Unity Events
    public void StartQuest(string questName)
    {
        SetQuestState(questName, ProgressState.Active);
    }
    public void CompleteQuest(string questName)
    {
        SetQuestState(questName, ProgressState.Complete);
    }
    public void StartTask(string taskName)
    {
        SetTaskState(taskName, ProgressState.Active);
    }
    public void CompleteTask(string taskName)
    {
        SetTaskState(taskName, ProgressState.Complete);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(QuestTracker))]
public class QuestTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        QuestTracker questTracker = (QuestTracker)target;
    }
}
#endif

