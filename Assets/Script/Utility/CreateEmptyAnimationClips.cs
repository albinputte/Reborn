using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateEmptyAnimationClips : MonoBehaviour
{
    public string folderPath = "Assets/Animations";  // Directory where the animations will be saved
    public string baseName = "MyAnimation";          // Base name for the animation clips
    public int clipCount = 5;                        // Number of clips to create

    [ContextMenu("Create Empty Animation Clips")]
    void CreateEmptyClips()
    {
        // Ensure the folder exists
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError($"Directory '{folderPath}' does not exist! Creating it now...");
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        // Create the specified number of animation clips
        for (int i = 1; i <= clipCount; i++)
        {
            string clipName = $"{baseName}{i}";
            string fullPath = Path.Combine(folderPath, $"{clipName}.anim");


            // Create an empty AnimationClip
            AnimationClip animClip = new AnimationClip();

            // Save the animation clip as an asset
            AssetDatabase.CreateAsset(animClip, fullPath);

            // Rename the asset to the desired clip name
            AssetDatabase.RenameAsset(folderPath, clipName);
        }

        // Save all the assets and refresh the asset database
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{clipCount} empty animation clips created in '{folderPath}' with base name '{baseName}'.");
    }

    public void Start()
    {
        CreateEmptyClips();
    }
}