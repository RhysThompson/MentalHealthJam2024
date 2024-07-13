using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Linq;
public class QuestHUD : StaticInstance<QuestHUD>
{
    public List<QuestHUDItem> questHUDListings;

    private void Start()
    {
        questHUDListings = GetComponentsInChildren<QuestHUDItem>(true).ToList();
        UpdateQuestHUD();
    }
    public void UpdateQuestHUD()
    {
        List<Quest> quests = QuestTracker.Instance.ActiveQuests;
        for (int i = 0; i < questHUDListings.Count; i++)
        {
            if(i < quests.Count)
            {
                questHUDListings[i].Open();
                string taskText = "";
                foreach (Task task in quests[i].ActiveTasks)
                { taskText += task.name + "\n"; }
                questHUDListings[i].ChangeText(quests[i].name, taskText);

            }
            else
            {
                questHUDListings[i].Close();
            }
        }
    }
    // Update the HUD when a quest is changed
    public void UpdateQuestHUD(Objective objective, ObjectiveState state)
    {
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

}
