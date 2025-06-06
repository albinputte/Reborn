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
    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Exit the game
    public void QuitGame()
    {
      Application.Quit();
    }
}