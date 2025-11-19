using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderTrigger : MonoBehaviour
{
    [Header("Scenes to LOAD when exiting this trigger")]
    public SceneField[] scenesToLoad;

    [Header("Scenes to UNLOAD when exiting this trigger")]
    public SceneField[] scenesToUnload;

    [Header("Tag that triggers this (e.g. Player)")]
    public string triggerTag = "Player";

    private bool isProcessing = false;

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("UwU");
        if (!other.CompareTag(triggerTag)) return;
        Debug.Log("UwU2");
        if (isProcessing) return;
        Debug.Log("UwU3");
        // Check if the trigger is allowed:
      
        
        isProcessing = true;

        // Load scenes
        if (scenesToLoad.Length > 0)
        {
     
            SceneManger.instance.LoadScenes(scenesToLoad);
        }

        // Unload scenes
        if (scenesToUnload.Length > 0)
        {
            SceneManger.instance.UnloadScenes(scenesToUnload);
        }

        // Allow re-triggering after a delay to avoid physics spam
        StartCoroutine(ResetCooldown());
    }

    /// <summary>
    /// Ensures the trigger only activates when appropriate:
    /// - If we want to load scenes, they must NOT already be loaded.
    /// - If we want to unload scenes, they MUST be loaded.
    /// </summary>
    private bool AreRequiredScenesInCorrectState()
    {
        foreach (var scene in scenesToLoad)
        {
            if (SceneManger.instance == null) return true;
            if (IsSceneLoaded(scene))
            {
                // Scene already loaded → no need to trigger
                return false;
            }
        }

        foreach (var scene in scenesToUnload)
        {
            if (!IsSceneLoaded(scene))
            {
                // Scene not loaded → unloading makes no sense
                return false;
            }
        }

        return true;
    }

    private bool IsSceneLoaded(SceneField scene)
    {
        var s = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.SceneName);
        return s.isLoaded;
    }

    private System.Collections.IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.6f); // small buffer
        isProcessing = false;
    }
}
