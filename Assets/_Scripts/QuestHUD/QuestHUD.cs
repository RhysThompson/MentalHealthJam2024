using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestHUD : StaticInstance<QuestHUD>
{
    public GameObject questHUDItemPrefab;
    public Dictionary<Quest, QuestHUDItem> questHUDItems = new Dictionary<Quest, QuestHUDItem>();
    public Transform toDoGroup;
    private static float delayForCheckMark = 1.5f;
    private void Start()
    {
        //questHUDListings = GetComponentsInChildren<QuestHUDItem>(true).ToList();
        QuestTracker.Instance.AddListener(gameObject);
        UpdateQuestHUD();
    }


    // Update the HUD when a quest is changed
    public void OnQuestUpdated(Objective objective)
    {
        if (objective.state == ProgressState.Complete && objective is Task)
        {
            Task task = objective as Task;
            Quest parentQuest = QuestTracker.Instance.GetQuestFromTask(task);
            TaskHUDItem taskHUDItem = questHUDItems[parentQuest].taskHUDItems[task];
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
        foreach (Quest quest in QuestTracker.Instance.GetActiveQuests())
        {
            QuestHUDItem HUDItem = Instantiate(questHUDItemPrefab, toDoGroup).GetComponent<QuestHUDItem>();
            HUDItem.quest = quest;
            questHUDItems[quest] = HUDItem;
            HUDItem.ChangeText(quest.name);
        }
    }

    public IEnumerator DestroyAfterAnimation(TaskHUDItem taskHUD)
    {
        taskHUD.PlayCompleteAnimation();
        yield return new WaitForSeconds(delayForCheckMark);
        CompleteQuestIfTasksAreDone(QuestTracker.Instance.GetQuestFromTask(taskHUD.task));
        if(taskHUD != null)
            Destroy(taskHUD.gameObject);
        UpdateQuestHUD();
    }
    public void CompleteQuestIfTasksAreDone(Quest quest)
    {
        StartCoroutine(CheckCompleteWait(quest));
    }
    public IEnumerator CheckCompleteWait(Quest quest)
    {
        yield return new WaitForSeconds(delayForCheckMark);
        if (quest.hasAllCompleteTasks())
        {
            QuestTracker.Instance.SetObjectiveState(quest, ProgressState.Complete);
        }
    }
}
