using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Helpers;
public class DialogueBoxUI : SingletonMenu<DialogueBoxUI>
{
    CanvasGroup CanvasGroup;

    private void Start()
    {
        CanvasGroup = GetComponentInParent<CanvasGroup>();
    }
    public override void Open()
    {
        base.Open();
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, 0.5f);
    }
    public override void Close()
    {
        CanvasGroup.DOFade(0, 0.5f);
        StartCoroutine(InvokeDelayed(base.Close, 0.5f));
    }
}
