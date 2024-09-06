using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SceneSwitcher : Singleton<SceneSwitcher>
{
    public static bool optsWithPause = false;
    private Animator transitionAnim;
    public List<RuntimeAnimatorController> transitionAnimations = new List<RuntimeAnimatorController>();
    public void Start()
    {
        transitionAnim = GetComponentInChildren<Animator>();
    }

    public void ChangeScene(string sceneName, int transitionIndex = 1)
    {
        if(transitionIndex==0)
        SceneManager.LoadSceneAsync(sceneName);
        else {
        transitionAnim.runtimeAnimatorController = transitionAnimations[transitionIndex-1];
        StartCoroutine(LoadLevel(sceneName));
        }
        //optsWithPause = false;
    }
    IEnumerator LoadLevel(string sceneName)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync(sceneName);
        transitionAnim.SetTrigger("Start");
    }
    IEnumerator LoadLevel(int sceneIndex)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync(sceneIndex);
        transitionAnim.SetTrigger("Start");
    }

/*
    public void OptionsMenu(string whoRanThis)
    {
        if (whoRanThis == "pausemenu")
        {
            optsWithPause = true;
        }
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
        
    }

    /// <summary>
    /// Used in the options menu
    /// </summary>
    
    public void GoBACK()
    {
        if (optsWithPause)
        {
            // The options menu was opened via the pause menu
            optsWithPause = false;
        }
        SceneManager.UnloadSceneAsync("Options");
    }*/

    public void ReturnToMenu()
    {
        ChangeScene("MainMenu");
    }
    
    public static void QuitGame()
    {
        #if !UNITY_EDITOR
			Application.Quit();
		#endif
		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
    }
    /// <summary>
    /// switch to a scene with given name ex. LoadLevel("Level 1");
    /// </summary>

        public void RestartScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextScene() // Used to move to next level
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
    }
}
