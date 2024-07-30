using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public AudioClip[] SpeakNoises;
    public void TriggerDialogue()
    {
        DialogueManager.Instance.SpeakNoises = SpeakNoises;
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
