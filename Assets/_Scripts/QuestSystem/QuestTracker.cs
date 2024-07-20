using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.ComponentModel;
using System.Collections;

[Serializable]
public class QuestTracker : Singleton<QuestTracker>
{
    public List<Quest> QuestData { get; private set; } = new List<Quest>();
    public List<Quest> ActiveQuests { get; private set; } = new List<Quest>();
    public List<Quest> CompleteQuests { get; private set; } = new List<Quest>();
    public static UnityEvent<Objective, ObjectiveState> OnQuestChanged = new UnityEvent<Objective, ObjectiveState>();
    public void Start()
    {
        QuestData = Resources.LoadAll<Quest>("Quests/").ToList();
        QuestData.ForEach(x => x.Start());

        if (!ValidateIDs())
        {
            Debug.LogError("Quest IDs are not unique. Please fix this in the quest Tracker by changing the ID's of non-unique quests");
        }
        // Trigger the OnQuestChanged event for all quests that are already active or complete.
        ActiveQuests.ForEach(activeQuest => OnQuestChanged?.Invoke(activeQuest,ObjectiveState.Started));
        CompleteQuests.ForEach(completeQuest => OnQuestChanged?.Invoke(completeQuest,ObjectiveState.Completed));
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
        OnQuestChanged?.Invoke(quest, ObjectiveState.Started);
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
        OnQuestChanged?.Invoke(quest, ObjectiveState.Completed);
    }
    //used by the Quest Script to automatically check if the quest is complete.
    //This was added here because the Quest script is a ScriptableObject and can't have a Coroutine.
    public void CheckComplete(Quest quest)
    {
        StartCoroutine(CheckCompleteWait(quest));
    }
    public IEnumerator CheckCompleteWait(Quest quest)
    {
        yield return new WaitForSeconds(1.5f);
        print("auto completing quest: " + quest.name);
        if (quest.CompleteTasks.Count == quest.TaskData.Count)
        {
            CompleteQuest(quest);
        }
    }

    public void CompleteTask(Task task) => task.CompleteTask();
    public void StartTask(Task task) => task.StartTask();
    public Quest IDToQuest(int QuestId) => QuestData.Find(x => x.id == QuestId);
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

public enum ObjectiveState
{
    None,
    Started,
    Completed
}