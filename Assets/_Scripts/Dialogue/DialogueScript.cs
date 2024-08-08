using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
[System.Serializable]
public class DialogueInstruction
{
    [TextArea(3, 10)]
    public string Text;
    public string Name;
    public bool ClearExistingText;
    public bool NewLine;
    public bool WaitForInput;
    public float StartDelay;
    public float PerLetterDelay = 0.05f;
    public VoiceTone voiceTone;
    public int eventIndex;
    //public DialogueActions Action;
    //public int StatBoostAmount;
    public DialogueInstruction Clone()
    {
        DialogueInstruction newObj = new DialogueInstruction();
        newObj.Text = Text;
        newObj.Name = Name;
        newObj.ClearExistingText = ClearExistingText;
        newObj.NewLine = NewLine;
        newObj.WaitForInput = WaitForInput;
        newObj.StartDelay = StartDelay;
        newObj.PerLetterDelay = PerLetterDelay;
        newObj.voiceTone = voiceTone;
        newObj.eventIndex = eventIndex;
        return newObj;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public List<DialogueInstruction> DialogueInstructions;
}

public class DialogueScript : MonoBehaviour
{
    public Dialogue DialogueObject;
    public Queue<DialogueInstruction> DialogueInstructions;
    public static bool InDialogue = false;
    private float DialogueDelayTimer = 0f;
    public TextMeshProUGUI DialogueBoxText;
    public TextMeshProUGUI DialogueBoxName;
    public RectTransform DialogueBox;
    public Dictionary<VoiceTone,List<AudioClip>> SpeakNoises;
    private bool skipTextAnim = false;
    [HideInInspector] public UnityEvent[] events;

    enum DialogueState
    {
        NONE,
        PREPARING,
        OPENINGBOX,
        WRITING,
        WAITING,
        WAITINGTOCLOSE,
        CLOSINGBOX
    }

    private DialogueState CurrentState = DialogueState.PREPARING;
    
    void Start()
    {
        DialogueBox.localScale = new Vector3(1, 0, 1);
        //DialogueBoxIndicator.SetActive(false);
    }

    void Update()
    {
        if (InDialogue)
        {
            switch (CurrentState)
            {
                case DialogueState.PREPARING:
                    if (GameManager.Instance.State == GameState.Talking)
                    {
                        CurrentState = DialogueState.OPENINGBOX;
                        DialogueBoxText.text = "";
                    }
                    break;

                case DialogueState.OPENINGBOX:

                    DialogueBoxUI.Instance.Open();
                    DialogueDelayTimer = 0.5f;
                    DialogueDelayTimer += DialogueInstructions.Peek().StartDelay;
                    CurrentState = DialogueState.WRITING;
                    events[DialogueInstructions.Peek().eventIndex]?.Invoke(); //invoke the event for the very first instruction
                    break;

                case DialogueState.WRITING:

                    DialogueDelayTimer -= Time.deltaTime;
                    DialogueBoxName.text = DialogueInstructions.Peek().Name;

                    if (DialogueInstructions.Peek().ClearExistingText)
                    {
                        DialogueBoxText.text = "";
                        DialogueInstructions.Peek().ClearExistingText = false;
                    }

                    if (DialogueInstructions.Peek().NewLine)
                    {
                        DialogueBoxText.text += "\n";
                        DialogueInstructions.Peek().NewLine = false;
                    }

                    while (DialogueDelayTimer <= 0)
                    {
                        if (SpeakNoises != null && SpeakNoises[DialogueInstructions.Peek().voiceTone].Count > 0) //play voice noises
                        {
                            AudioSystem.Instance.SpeakWordsOnLoop(SpeakNoises[DialogueInstructions.Peek().voiceTone].ToArray<AudioClip>());
                        }

                        if (DialogueInstructions.Peek().Text.Length > 0)
                        {
                            DialogueBoxText.text += DialogueInstructions.Peek().Text[0];
                            DialogueInstructions.Peek().Text = DialogueInstructions.Peek().Text.Remove(0, 1);
                            if(skipTextAnim)
                                DialogueDelayTimer = 0;
                            else
                                DialogueDelayTimer += DialogueInstructions.Peek().PerLetterDelay;
                        }
                        else
                        {
                            skipTextAnim = false;
                            DialogueInstructions.Dequeue(); // remove the instruction that was just completed
                            
                            if (DialogueInstructions.Count >= 1)
                            {
                                if (DialogueInstructions.Peek().WaitForInput)
                                    CurrentState = DialogueState.WAITING;

                                DialogueDelayTimer = DialogueInstructions.Peek().StartDelay;
                            }
                            else if (DialogueInstructions.Count == 0)
                            {
                                CurrentState = DialogueState.WAITINGTOCLOSE;
                            }
                            
                            break;
                        }
                    }
                    break;

                case DialogueState.WAITING:
                    AudioSystem.Instance.StillSpeaking = false;
                    break;
                case DialogueState.WAITINGTOCLOSE:
                    AudioSystem.Instance.StillSpeaking = false;
                    break;
                case DialogueState.CLOSINGBOX:
                    DialogueBoxUI.Instance.Close();

                    InDialogue = false;
                    GameManager.Instance.ChangeState(GameState.Playing);
                    CurrentState = DialogueState.NONE;
                    print("End of conversation");
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                OnInteract();
        }
    }
    public void OnInteract()
    {
        switch (CurrentState)
        {
            case DialogueState.WRITING:
                skipTextAnim = true;
                break;

            case DialogueState.WAITING:
                CurrentState = DialogueState.WRITING;
                events[DialogueInstructions.Peek().eventIndex]?.Invoke(); //invoke the event for the next instruction
                break;
            case DialogueState.WAITINGTOCLOSE:
                CurrentState = DialogueState.CLOSINGBOX;
                break;
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        DialogueInstructions = new Queue<DialogueInstruction>();
        foreach (DialogueInstruction i in dialogue.DialogueInstructions)
            DialogueInstructions.Enqueue(i.Clone());

        GameManager.Instance.ChangeState(GameState.Talking);
        InDialogue = true;
        CurrentState = DialogueState.PREPARING;
    }
}
