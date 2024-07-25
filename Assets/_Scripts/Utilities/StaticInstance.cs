using UnityEngine;


/* A collection of classes to be inherited from to make organization and referencing of some classes easier.
 * Inheriting from any of these classes allows that class to be referenced as (ClassName).Instance
 * ex. PauseMenuManager.Instance.Open()
 * This would open the pause menu as long as an object with the PauseMenuManager component is in the scene,
 * However only one PauseMenuManager can exist at one time.
 * These classes are taken from this video: https://www.youtube.com/watch?v=tE1qH8OxO2Y&t=635s
 */


/// <summary>
/// saves the class as a Static Instance. The class can then be referenced as (ClassName).Instance, 
/// however only one instance of this class may exist at once.
/// A static instance is similar to a singleton, but instead of destroying any new
/// instances, it overrides the current instance. This is handy for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// versions created, leaving the original instance intact
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}

/// <summary>
/// Finally we have a persistent version of the singleton. This will survive through scene
/// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
/// where music plays through loading screens, etc
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

