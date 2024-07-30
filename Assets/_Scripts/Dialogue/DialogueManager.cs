using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//based on Dialogue Tutorial by Brackeys: https://www.youtube.com/watch?v=_nRzoTzeyxU
public class DialogueManager : StaticInstance<DialogueManager>
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    /// <summary>
    /// The speed at which the text is displayed, ranges from 1 to 100
    /// </summary>
    public int textSpeed = 1;
    private Queue<string> sentences;
    private StarterAssetsInputs playerInputs;
    [HideInInspector] public AudioClip[] SpeakNoises;
    private void Start()
    {
        sentences = new Queue<string>();
        playerInputs = FindFirstObjectByType<StarterAssetsInputs>();
        if (textSpeed < 1 || textSpeed > 100)
        {
            Debug.LogError("Text speed must be between 1 and 100");
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        nameText.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DialogueBoxUI.Instance.Open();
        DisplayNextSentence();

        //turn off player movement and unlock cursor
        playerInputs.moveDisabled = true;
        Cursor.lockState = CursorLockMode.None;
        playerInputs.cursorLocked = false;
        playerInputs.cursorInputForLook = false;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (SpeakNoises != null && SpeakNoises.Length > 0)
        {
            AudioSystem.Instance.SpeakWordsOnLoop(SpeakNoises);
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        int accum = 0;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            accum = 0;
            dialogueText.text += letter;
            while (accum != 100 - textSpeed)
            {
                accum++;
                yield return new WaitForEndOfFrame();
            }
        }
        AudioSystem.Instance.StillSpeaking = false;
    }
    public void EndDialogue() {
        Debug.Log("End of conversation");
        DialogueBoxUI.Instance.Close();
        playerInputs.moveDisabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInputs.cursorLocked = true;
        playerInputs.cursorInputForLook = true;
    }
}
