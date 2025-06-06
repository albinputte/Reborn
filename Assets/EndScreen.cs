using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject UiCanvas;
    [SerializeField] private GameObject ToolTipCanvas;
    [SerializeField] private GameObject EndCanvas;
    public GameObject Inputmanger;
    public PlayerInput inputs;

    public void Start()
    {
     
    }

    public void ContinueToPlayButton()
    {
       
        EndCanvas.SetActive(false);
 
     
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
    }



}
