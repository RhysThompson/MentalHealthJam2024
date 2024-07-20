using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public KeyCode interactButton = KeyCode.E;
    public string promptText;
    public bool overlappingPlayer = false;

    // Update is called once per frame
    void Update()
    {
        if (overlappingPlayer && Input.GetKeyDown(interactButton))
        {
            OnInteract?.Invoke();
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
