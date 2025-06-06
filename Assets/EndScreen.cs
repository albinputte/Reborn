using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject UiCanvas;
    [SerializeField] private GameObject ToolTipCanvas;
    [SerializeField] private GameObject EndCanvas;


    public void ContinueToPlayButton()
    {
        Time.timeScale = 1.0f;
   
        EndCanvas.SetActive(false);
        Cursor.visible = false;
     
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
    }



}
