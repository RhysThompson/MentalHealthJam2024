using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskHUDItem : MonoBehaviour
{
    public TextMeshProUGUI taskName;
    public Animator checkBoxAnimator;
    public Task task;
    // Start is called before the first frame update
    void Start()
    {
        taskName = transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
        checkBoxAnimator = GetComponentInChildren<Animator>();
    }

    public void ChangeText(string newTaskName)
    {
        taskName.text =newTaskName;
    }

    public void PlayCompleteAnimation()
    {
        checkBoxAnimator.SetTrigger("CheckOff");
    }
    public bool isFinishedAnimation()
    {
        return (checkBoxAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}
