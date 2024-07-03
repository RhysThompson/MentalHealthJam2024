using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public abstract class Objective : ScriptableObject
{
    public int id;
    public new string name;
    public string description;
    public bool IsComplete;
    public UnityEvent OnComplete = new UnityEvent();
    public UnityEvent OnStart = new UnityEvent();
}
