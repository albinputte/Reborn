using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public static SceneManger instance;

    [SerializeField] private SceneField[] essentialScenesToLoad;

    public Action OnAllEssentialScenesLoaded;

    private int loadedEssentialCount = 0;
    private readonly List<AsyncOperation> loadingOperations = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (var scene in essentialScenesToLoad)
        {
            if (!IsSceneLoaded(scene))
            {
                var op = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);
                loadingOperations.Add(op);
                op.completed += OnEssentialSceneLoaded;
            }
            else
            {
                loadedEssentialCount++;
            }
        }

        // If everything was already loaded
        if (loadedEssentialCount == essentialScenesToLoad.Length)
            OnAllEssentialScenesLoaded?.Invoke();
    }

    private bool IsSceneLoaded(SceneField scene)
    {
        return SceneManager.GetSceneByName(scene.SceneName).isLoaded;
    }

    private void OnEssentialSceneLoaded(AsyncOperation op)
    {
        loadedEssentialCount++;

        if (loadedEssentialCount == essentialScenesToLoad.Length)
        {
            OnAllEssentialScenesLoaded?.Invoke();
        }
    }

    public void LoadScenes(SceneField[] scenesToLoad)
    {
        foreach (var scene in scenesToLoad)
        {
            Debug.Log("UwULoad");
            if (!IsSceneLoaded(scene))
            {
                Debug.Log("UwULoad1");
                var op = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);
                loadingOperations.Add(op);
            }
        }
    }

    public void UnloadScenes(SceneField[] scenesToUnload)
    {
        foreach (var scene in scenesToUnload)
        {
            Debug.Log("UwUunLoad");
            if (IsSceneLoaded(scene))
            {
                Debug.Log("UwUunLoad1");
                var op = SceneManager.UnloadSceneAsync(scene.SceneName);
                loadingOperations.Add(op);
            }
        }
    }
}
