using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static ToolTipSystem;

public class UIToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 10)]
    public string header;
    [TextArea(3, 10)]
    public string content;
    [TextArea(3, 10)]
    public string values;
    public bool IsActive;
    public static bool IsDragging;

    private RectTransform rectTransform;
    private void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }

    public void Dragging()
    {
        UIToolTipTrigger.IsDragging =true;
        TooltipCoroutineManager.StopCurrentTooltipCoroutine();
        ToolTipSystem.Hide();
        IsActive = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(header) && !UIToolTipTrigger.IsDragging)
        {
            TooltipCoroutineManager.StartTooltipCoroutine(this, ShowTooltipAfterDelay(0.2f));
            IsActive = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive && !UIToolTipTrigger.IsDragging)
        {
            TooltipCoroutineManager.StopCurrentTooltipCoroutine();
            ToolTipSystem.Hide();
            IsActive = false;
        }
    }

    private IEnumerator ShowTooltipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToolTipSystem.Show(values, content, header, rectTransform); // Add rectTransform parameter
    }

    private void OnDisable()
    {
        if (IsActive)
        {
            TooltipCoroutineManager.StopCurrentTooltipCoroutine();
            ToolTipSystem.Hide();
            IsActive = false;
        }
    }

}
