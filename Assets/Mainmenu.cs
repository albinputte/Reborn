using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    // This method can be called from a UI Button
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
