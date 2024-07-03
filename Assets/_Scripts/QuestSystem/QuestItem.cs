using UnityEngine;
using UnityEngine.Events;

public class QuestItem : MonoBehaviour
{
    public bool overlappingPlayer = false;
    public UnityEvent OnInteract;
    public KeyCode interactButton = KeyCode.E;
    public string promptText;
    // Update is called once per frame
    void Update()
    {
        if(overlappingPlayer && Input.GetKeyDown(interactButton))
        {
            OnInteract?.Invoke();
            print("Quest Item Collected");
            Destroy(gameObject);
            ButtonPrompts.Instance.Close();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            overlappingPlayer = true;
            ButtonPrompts.Instance.Prompt(interactButton, promptText);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            overlappingPlayer = false;
            ButtonPrompts.Instance.Close();
        }
    }
}
