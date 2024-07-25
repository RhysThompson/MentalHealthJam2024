using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ButtonPrompts : SingletonMenu<ButtonPrompts>
{
    TextMeshProUGUI text;
    Image image;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
    }
    public void Prompt(KeyCode keyName, string promptText)
    {
        image.sprite = Resources.Load<Sprite>("Button Prompts/" + keyName.ToString() +"_Key_Dark");
        text.text = promptText;
        Open();
    }
}
