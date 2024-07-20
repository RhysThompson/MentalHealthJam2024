using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
public class QuestHUD : StaticInstance<QuestHUD>
{
    public GameObject questHUDItemPrefab;
    public Dictionary<Quest, QuestHUDItem> questHUDItems = new Dictionary<Quest, QuestHUDItem>();
    public Transform toDoGroup;
    private float delayForCheckMark = 1.5f;
    private void Start()
    {
        //questHUDListings = GetComponentsInChildren<QuestHUDItem>(true).ToList();
        UpdateQuestHUD();
    }

    // Update the HUD when a quest is changed
    public void UpdateQuestHUD(Objective objective, ObjectiveState state)
    {
        if (state == ObjectiveState.Completed && objective is Task)
        {
            Task task = objective as Task;
            TaskHUDItem taskHUDItem = questHUDItems[task.quest].taskHUDItems[task];
            StartCoroutine(DestroyAfterAnimation(taskHUDItem));
        }
        else
            UpdateQuestHUD();
    }

    public void UpdateQuestHUD()
    {
        foreach (QuestHUDItem item in questHUDItems.Values)
        {
            Destroy(item.gameObject);
        }
        questHUDItems.Clear();

        GenerateQuests();
        questHUDItems.Values.ToList().ForEach(q => q.GenerateTasks());
    }
    public void GenerateQuests()
    {
        List<Quest> quests = QuestTracker.Instance.ActiveQuests;
        foreach (Quest quest in quests)
        {
            QuestHUDItem HUDItem = Instantiate(questHUDItemPrefab, toDoGroup).GetComponent<QuestHUDItem>();
            HUDItem.quest = quest;
            questHUDItems[quest] = HUDItem;
            HUDItem.ChangeText(quest.name);
        }
    }

    public void OnEnable()
    {
        QuestTracker.OnQuestChanged.AddListener(UpdateQuestHUD);
    }
    public void OnDisable()
    {
        QuestTracker.OnQuestChanged.RemoveListener(UpdateQuestHUD);
    }

    public IEnumerator DestroyAfterAnimation(TaskHUDItem task)
    {
        task.PlayCompleteAnimation();
        yield return new WaitForSeconds(delayForCheckMark); // Add a delay of 1 second
        Destroy(task.gameObject);
        UpdateQuestHUD();
    }
}
