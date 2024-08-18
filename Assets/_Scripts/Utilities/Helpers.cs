using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers 
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static IEnumerator InvokeDelayed(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}

