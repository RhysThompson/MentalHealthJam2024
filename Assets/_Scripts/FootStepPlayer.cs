using UnityEngine;
using FMODUnity;
/// <summary>
/// FMOD sounds can't be played from ThirdPersonCharacter.cs, so this script is used to play footstep sounds.
/// </summary>
public class FootStepPlayer : MonoBehaviour
{
    [SerializeField]
    private EventReference footstepEventPath;

    private FMOD.Studio.EventInstance footstepEvent;

    public void OnStep()
    {
        footstepEvent = RuntimeManager.CreateInstance(footstepEventPath);
        footstepEvent.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        footstepEvent.start();
    }
}
