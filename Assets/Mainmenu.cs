using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    // Load the game scene
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Exit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); // This will show in the editor but not in a built game
    }
}