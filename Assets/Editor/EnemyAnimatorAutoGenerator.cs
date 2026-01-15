using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;

public static class EnemyAnimatorGenerator
{
    [MenuItem("Tools/Enemy/Generate Animator (Select Controller Script)")]
    public static void Generate()
    {
        // 1️⃣ Get selected script
        MonoScript script = Selection.activeObject as MonoScript;
        if (script == null)
        {
            Debug.LogError("❌ Select an EnemyBaseController script in the Project window.");
            return;
        }

        Type controllerType = script.GetClass();
        if (controllerType == null ||
            !typeof(EnemyBaseController).IsAssignableFrom(controllerType))
        {
            Debug.LogError("❌ Selected script is not an EnemyBaseController.");
            return;
        }

        // 2️⃣ Pull anim names from controller
        MethodInfo animMethod = controllerType.GetMethod(
            "GetAnimNames",
            BindingFlags.Public | BindingFlags.Static
        );

        if (animMethod == null)
        {
            Debug.LogError($"❌ {controllerType.Name} must implement static GetAnimNames()");
            return;
        }

        string[] animNames = animMethod.Invoke(null, null) as string[];
        if (animNames == null || animNames.Length == 0)
        {
            Debug.LogError("❌ GetAnimNames returned no animation names.");
            return;
        }

        // 3️⃣ Paths
        string enemyName = controllerType.Name.Replace("EnemyController", "");
        string basePath = $"Assets/Animations/{enemyName}";
        string controllerPath = $"{basePath}/{enemyName}Animator.controller";

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        // 4️⃣ Create Animator Controller
        AnimatorController animator =
            AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        AnimatorStateMachine sm = animator.layers[0].stateMachine;

        // Clear default state Unity adds
        sm.states = Array.Empty<ChildAnimatorState>();

        AnimatorState defaultState = null;

        // 5️⃣ Create clips + states + assign clips to AC
        foreach (string animName in animNames)
        {
            string clipPath = $"{basePath}/{animName}.anim";

            AnimationClip clip =
                AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

            if (clip == null)
            {
                clip = new AnimationClip
                {
                    frameRate = 12f,
                    wrapMode = WrapMode.Loop
                };

                AssetDatabase.CreateAsset(clip, clipPath);
            }

            // 👉 THIS IS WHERE THE CLIP IS ADDED TO THE AC
            AnimatorState state = sm.AddState(animName);
            state.motion = clip;

            if (defaultState == null)
                defaultState = state;
        }

        // 6️⃣ Set default state
        sm.defaultState = defaultState;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"✅ Animator + clips generated for {enemyName}");
    }
}
