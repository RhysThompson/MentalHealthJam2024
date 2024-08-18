using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
[System.Serializable]
///<summary>
///This class is used to create a QuestItem object that can be used to trigger events when a quest or task makes progress.
///If the Object starts disabled, it will not subscribe to the QuestTracker, and the set events will not trigger.
///</summary> 
public abstract class QuestItem : MonoBehaviour
{
    public string QuestOrTaskName;
    public ProgressState state = ProgressState.Inactive;
    public void Start()
    {
        QuestTracker.Instance.AddListener(gameObject);
    }
    public void OnDestroy()
    {
        if(QuestTracker.Instance != null)
            QuestTracker.Instance.RemoveListener(gameObject);
    }
    public abstract void OnQuestUpdated(Objective objective);
}