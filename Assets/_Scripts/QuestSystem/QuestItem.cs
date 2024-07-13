using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
[System.Serializable]
///<summary>
///This class is used to create a QuestItem object that can be used to trigger events when a quest or task makes progress.
///If the Object starts disabled, it will not subscribe to the QuestTracker, and the set events will not trigger.
///</summary> 
public class QuestItem : MonoBehaviour
{
    public ItemResponse[] Responses;
    private List<UnityAction<Objective, ObjectiveState>> actions = new List<UnityAction<Objective, ObjectiveState>>();
    
    public void OnEnable()
    {
        SubscribeListeners();
    }
    public void OnDisable()
    {
        UnSubscribeListeners();
    }
    public void SubscribeListeners()
    {
        foreach (ItemResponse response in Responses)
        {
            UnityAction<Objective, ObjectiveState> questFilter = (objective, state) => 
                { if (objective == response.QuestOrTask && state == response.OnStateChange)
                    {
                        response.Listener?.Invoke(objective, state);
                        print(state.ToString() + " " + objective.name);
                    }
                };
            QuestTracker.OnQuestChanged.AddListener(questFilter);
            actions.Add(questFilter);
            print("Subscribed to QuestTracker" + response.ToString());
        }
    }
    public void UnSubscribeListeners()
    {
       foreach (UnityAction<Objective, ObjectiveState> action in actions)
        {
            QuestTracker.OnQuestChanged.RemoveListener(action);
        }
        actions.Clear();
    }
}

[System.Serializable]
public class ItemResponse
{
    public Objective QuestOrTask;
    public ObjectiveState OnStateChange;
    public UnityEvent<Objective, ObjectiveState> Listener;
}

[CustomEditor(typeof(QuestItem))]
public class QuestItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
