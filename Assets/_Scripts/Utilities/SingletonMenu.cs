using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// Base for menus. Note that multiple
/// </summary>
[RequireComponent(typeof(Canvas))]
public abstract class SingletonMenu<T> : Singleton<T> where T : SingletonMenu<T>
{
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public bool isOpen = false;
    public new void Awake()
    {
        base.Awake();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
    public virtual void Open()
    { 
        canvas.enabled=true;
        isOpen = true;
    }
    public virtual void Close()
    { 
        canvas.enabled = false;
        isOpen = false;
    }
    public virtual void Toggle()
    {
        if(isOpen)
            Close();
        else
            Open();
    }
}
