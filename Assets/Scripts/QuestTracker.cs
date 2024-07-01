using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class QuestTracker : Singleton<QuestTracker>
{
    public List<Quest> QuestData = new List<Quest>();
    public List<Quest> ActiveQuests { get; private set; } = new List<Quest>();
    public List<Quest> CompleteQuests { get; private set; } = new List<Quest>();

    public void Start()
    {
        if (!ValidateIDs())
        {
            Debug.LogError("Quest IDs are not unique. Please fix this in the quest Tracker by changing the ID's of non-unique quests");
        }
    }
    public bool ValidateIDs()
    {
        return QuestData.GroupBy(x => x.id).All(g => g.Count() == 1);
    }
    
    public void StartQuest(Quest quest)
    {
        if(ActiveQuests.Contains(quest) || CompleteQuests.Contains(quest))
        {
            print("Attempted to start an active or complete quest");
            return;
        }
        quest.OnStart?.Invoke();
        ActiveQuests.Add(quest);
    }
    
    public void CompleteQuest(Quest quest)
    {
        if (CompleteQuests.Contains(quest))
        {
            print("Attempted to complete a quest that was already completed");
            return;
        }
        if (!ActiveQuests.Contains(quest))
        {
            print("Attempted to complete a quest that was not active");
            return;
        }
        quest.OnComplete?.Invoke();
        ActiveQuests.Remove(quest);
        CompleteQuests.Add(quest);
    }
    public void StartQuest(int QuestId)
    {
        StartQuest(IDToQuest(QuestId));
    }
    public void CompleteQuest(int QuestId)
    {
        CompleteQuest(IDToQuest(QuestId));
    }
    public Quest IDToQuest(int QuestId)
    {
        return QuestData.Find(x => x.id == QuestId);
    }

}

[Serializable]
public abstract class Objective
{
    public int id;
    public string name;
    public string description;
    public bool IsComplete;
    public UnityEvent OnComplete = new UnityEvent();
    public UnityEvent OnStart = new UnityEvent();
}

[Serializable]
public class Quest : Objective
{
    List<Task> tasks = new List<Task>();
}

[Serializable]
public class Task : Objective
{

}


//I made this custom window editor but it's no longer neccesary. I kept it here in case I ever need to make another editor

[CustomEditor(typeof(QuestTracker))]
public class QuestTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        QuestTracker questTracker = (QuestTracker)target;
    }
}
