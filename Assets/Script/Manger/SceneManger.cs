using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public static SceneManger instance;
    private AsyncOperation[] loadingOperations;
    [SerializeField] private SceneField[] essentialScenesToLoad; // Scenes that must be loaded

    public Action OnAllEssentialScenesLoaded;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        loadingOperations = new AsyncOperation[essentialScenesToLoad.Length];

        for (int i = 0; i < essentialScenesToLoad.Length; i++)
        {
            if (!IsSceneLoaded(essentialScenesToLoad[i]))
            {
                loadingOperations[i] = SceneManager.LoadSceneAsync(
                    essentialScenesToLoad[i],
                    LoadSceneMode.Additive
                );

                loadingOperations[i].completed += OnEssentialSceneLoaded;
            }
        }
    }

    private bool IsSceneLoaded(SceneField scene)
    {
        Scene s = SceneManager.GetSceneByName(scene.SceneName);
        return s.isLoaded;
    }

    private void OnEssentialSceneLoaded(AsyncOperation op)
    {
        // Check if all essential scenes are now loaded
        int count = 0;
        for (int i = 0; i < essentialScenesToLoad.Length; i++)
        {
            if (IsSceneLoaded(essentialScenesToLoad[i]))
            {
                count++;
            }
        }

        if (count == essentialScenesToLoad.Length)
        {
            OnAllEssentialScenesLoaded?.Invoke();
        }
    }
}
