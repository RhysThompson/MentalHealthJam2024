using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.ComponentModel;

[Serializable]
public class QuestTracker : Singleton<QuestTracker>
{
    public List<Quest> QuestData { get; private set; } = new List<Quest>();
    public List<Quest> ActiveQuests { get; private set; } = new List<Quest>();
    public List<Quest> CompleteQuests { get; private set; } = new List<Quest>();
    public void Start()
    {
        if (!ValidateIDs())
        {
            Debug.LogError("Quest IDs are not unique. Please fix this in the quest Tracker by changing the ID's of non-unique quests");
        }
        QuestData = Resources.LoadAll<Quest>("Quests/").ToList();
        QuestData.ForEach(x => x.Start());
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
        QuestHUD.Instance.UpdateQuestHUD();
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
        print("Completed Quest: " + quest.name);
        QuestHUD.Instance.UpdateQuestHUD();
    }

    public void CompleteTask(Task task)
    {
        task.quest.CompleteTask(task);
    }
    public void StartTask(Task task)
    {
        task.quest.StartTask(task);
    }
    public Quest IDToQuest(int QuestId)
    {
        return QuestData.Find(x => x.id == QuestId);
    }

}

[CustomEditor(typeof(QuestTracker))]
public class QuestTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        QuestTracker questTracker = (QuestTracker)target;
    }
}
