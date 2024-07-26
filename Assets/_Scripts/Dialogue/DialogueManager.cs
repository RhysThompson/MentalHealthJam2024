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
    private Queue<string> sentences;
    private StarterAssetsInputs playerInputs;
    private void Start()
    {
        sentences = new Queue<string>();
        playerInputs = FindFirstObjectByType<StarterAssetsInputs>();
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
        dialogueText.text = sentence;
        print(sentence);
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
