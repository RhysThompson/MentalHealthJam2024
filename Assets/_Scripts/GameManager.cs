using AYellowpaper.SerializedCollections;
using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum GameState
{
    Starting = 0,
    Paused = 1,
    Playing = 2,
    Talking = 3,
    Minigame = 4,
}
/// <summary>
/// Base for a Game Manager. Mainly features: State machine for switching between game states.
/// I'm not sure if this is needed so it's not currently used in the project, but it's here if someone
/// wants to use it.
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public static GameState lastGameState = GameState.Starting;
    private StarterAssetsInputs playerInputs;
    public List<string> keyItems = new List<string>();
    public GameState State { get; private set; }
    /// <summary>
    /// Stops all state changes from the first state to any of the listed states. OnBeforeStateChanged will not be called.
    /// </summary>
    [SerializedDictionary("Can't change from this State", "To these States")]
    [SerializeField] private SerializedDictionary<GameState,List<GameState>> ForbidenStateTransitions;
    // Kick the game off with the first state
    void Start()
    {
        playerInputs = FindFirstObjectByType<StarterAssetsInputs>();
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState) {
        if (isValidTransition(State,newState)==false) return;

        lastGameState = State;
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Talking:
                DisableMovement();
                break;
            case GameState.Paused:
                break;
            case GameState.Playing:
                EnableMovement();
                break;
            case GameState.Minigame:
                StartMinigame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }
    public bool isValidTransition(GameState oldState, GameState newState)
    {
        bool inForbiddenTable = ForbidenStateTransitions.ContainsKey(oldState) && ForbidenStateTransitions[oldState].Contains(newState);
        bool isRepeat = oldState == newState;
        return !(inForbiddenTable || isRepeat);
    }
    ///automatically resumes game when exiting the pause GameState
    
    private void HandleStarting() {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state
        
    }
    public void DisableMovement()
    {
        playerInputs.moveDisabled = true;
        playerInputs.jumpDisabled = true;
        Cursor.lockState = CursorLockMode.None;
        playerInputs.cursorLocked = false;
        playerInputs.cursorInputForLook = false;
    }
    public void EnableMovement()
    {
        playerInputs.moveDisabled = false;
        playerInputs.jumpDisabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInputs.cursorLocked = true;
        playerInputs.cursorInputForLook = true;
    }
    public void StartMinigame()
    {
        DisableMovement();
        // Do some setup for the minigame
    }
}

