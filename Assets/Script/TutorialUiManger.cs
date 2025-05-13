using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUiManger : MonoBehaviour
{
 
    public static TutorialUiManger Instance;

    [Header("UI Elements")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    [SerializeField]
    private RectTransform panelRect;
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

       
        messagePanel.SetActive(false);
    }

  
    public void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }

   
    public void ClearMessage()
    {
        messagePanel.SetActive(false);
        messageText.text = "";
    }
    public void SetPanelPosition(MessagePosition position)
    {
        if (panelRect == null) return;

        switch (position)
        {
            case MessagePosition.Top:
                panelRect.anchorMin = new Vector2(0.5f, 1f);
                panelRect.anchorMax = new Vector2(0.5f, 1f);
                panelRect.pivot = new Vector2(0.5f, 1f);
                panelRect.anchoredPosition = new Vector2(0f, -50f);
                break;

            case MessagePosition.Center:
                panelRect.anchorMin = new Vector2(0.5f, 0.5f);
                panelRect.anchorMax = new Vector2(0.5f, 0.5f);
                panelRect.pivot = new Vector2(0.5f, 0.5f);
                panelRect.anchoredPosition = Vector2.zero;
                break;

            case MessagePosition.Bottom:
                panelRect.anchorMin = new Vector2(0.5f, 0f);
                panelRect.anchorMax = new Vector2(0.5f, 0f);
                panelRect.pivot = new Vector2(0.5f, 0f);
                panelRect.anchoredPosition = new Vector2(0f, 50f);
                break;
        }
    }


    public void SetPanelPositionCustom(Vector2 anchoredPosition)
    {
        panelRect.anchoredPosition = anchoredPosition;
    }

    public enum MessagePosition
    {
        Top,
        Center,
        Bottom
    }
}


