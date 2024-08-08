using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum VoiceTone
{
    Neutral,
    Happy,
    Sad,
    Angry,
    Scared,
    Surprised,
}
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializedDictionary("ToneKey", "VoiceClips")]
    public SerializedDictionary<VoiceTone, List<AudioClip>> SpeakNoises;
    private DialogueScript DialogueScript;
    public UnityEvent[] events;
    private void Start()
    {
        DialogueScript = FindFirstObjectByType<DialogueScript>();

    }
    public void TriggerDialogue()
    {
        DialogueScript.SpeakNoises = SpeakNoises;
        DialogueScript.events = events;
        DialogueScript.StartDialogue(dialogue);

    }
    public void testEvent(string text)
    {
        Debug.Log("Event Triggered "+text);
    }
}

#if UNITY_EDITOR
public class DialogueTriggerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueTrigger myScript = (DialogueTrigger)target;
        if (GUILayout.Button("Trigger Dialogue"))
        {
            myScript.TriggerDialogue();
        }
    }
}
#endif