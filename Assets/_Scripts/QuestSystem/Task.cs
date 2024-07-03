using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "Task", menuName = "QuestSystem/Task")]
public class Task : Objective
{
    [HideInInspector] public Quest quest;

    public void Start()
    {
        Quest[] quests = Resources.LoadAll<Quest>("Quests");
        quest = Array.Find(quests, q => q.TaskData.Contains(this));
    }
}
