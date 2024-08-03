using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public AudioClip[] SpeakNoises;
    public DialogueScript DialogueScript;
    private void Start()
    {
        DialogueScript = FindFirstObjectByType<DialogueScript>();
    
    }
    public void TriggerDialogue()
    {
        DialogueScript.SpeakNoises = SpeakNoises;
        DialogueScript.StartDialogue(dialogue);

        //DialogueManager.Instance.SpeakNoises = SpeakNoises;
        //DialogueManager.Instance.StartDialogue(dialogue);
    }
}
