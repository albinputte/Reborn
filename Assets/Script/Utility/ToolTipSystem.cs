using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem current;
    public float animationDuration = 0.1f;
    private Coroutine currentAnimationCoroutine;
    public ToolTip tooltip;
    private PlayerInput playerInput;

    public void Awake()
    {
        current = this;
        tooltip.transform.localScale = Vector3.zero; // Start hidden
    }
    private void Start()
    {

        HideImmediate();

    }

  

    private void HideTooltip(InputAction.CallbackContext ctx)
    {
        HideImmediate();
    }
    public static void Show(string values, string content, string header = "", RectTransform targetRect = null)
    {
        current.tooltip.SetText(values, content, header);
        RectTransform tooltipRect = current.tooltip.GetComponent<RectTransform>();
        Vector3 desiredPosition = Vector3.zero;
        Vector2 newPivot;

        if (targetRect != null)
        {
            Vector3[] corners = new Vector3[4];
            targetRect.GetWorldCorners(corners); // [0]=BL, [1]=TL, [2]=TR, [3]=BR

            // Use screen midpoint comparison
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, targetRect.position);
            bool isRightSide = screenPos.x > (Screen.width / 2f);

            if (isRightSide)
            {
                newPivot = new Vector2(1f, 0f); // bottom-right
                desiredPosition = corners[1] + new Vector3(20f, -20f, 0f); // TopLeft
            }
            else
            {
                newPivot = new Vector2(0f, 0f); // bottom-left
                desiredPosition = corners[2] + new Vector3(-20f, -20f, 0f); // TopRight
            }
        }
        else
        {
            desiredPosition = Input.mousePosition;
            newPivot = new Vector2(0f, 0f);
        }

        tooltipRect.pivot = newPivot;
        tooltipRect.position = desiredPosition;

        tooltipRect.localScale = Vector3.zero;

        if (current.currentAnimationCoroutine != null)
            current.StopCoroutine(current.currentAnimationCoroutine);

        current.currentAnimationCoroutine = current.StartCoroutine(current.FadeInAndShowTooltip());
        current.tooltip.gameObject.SetActive(true);
    }









    public static void HideImmediate()
    {
        current.tooltip.transform.localScale = Vector3.zero;
        current.tooltip.gameObject.SetActive(false);
        current.tooltip.UnlockPivot();
    }
    public static class TooltipCoroutineManager
    {
        private static Coroutine currentTooltipCoroutine;
        private static MonoBehaviour coroutineHost;

        public static void StartTooltipCoroutine(MonoBehaviour host, IEnumerator coroutine)
        {
            // Stop the previous coroutine if it exists
            if (currentTooltipCoroutine != null && coroutineHost != null)
            {
                coroutineHost.StopCoroutine(currentTooltipCoroutine);
            }

            coroutineHost = host;
            currentTooltipCoroutine = host.StartCoroutine(coroutine);
        }

        public static void StopCurrentTooltipCoroutine()
        {
            if (currentTooltipCoroutine != null && coroutineHost != null)
            {
                coroutineHost.StopCoroutine(currentTooltipCoroutine);
                currentTooltipCoroutine = null;
                coroutineHost = null;
            }
        }

    }
    public static void Hide()
    {
        // Start the fade-out and shrink animation
        if (current.currentAnimationCoroutine != null)
            current.StopCoroutine(current.currentAnimationCoroutine);

        current.currentAnimationCoroutine = current.StartCoroutine(current.ShrinkAndHideTooltip());
        current.tooltip.UnlockPivot();
    }

    private IEnumerator FadeInAndShowTooltip()
    {
        tooltip.gameObject.SetActive(true);
        Vector3 targetScale = Vector3.one;
        Vector3 initialScale = Vector3.zero;
        float time = 0f;

        while (time < animationDuration)
        {
            tooltip.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        tooltip.transform.localScale = targetScale; // Ensure final scale
    }

    private IEnumerator ShrinkAndHideTooltip()
    {
        Vector3 initialScale = tooltip.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float time = 0f;

        while (time < animationDuration)
        {
            tooltip.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        tooltip.transform.localScale = targetScale;
        tooltip.gameObject.SetActive(false);
    }
    public static void UpdateContent(string values, string content, string header = "")
    {
        if (current == null || current.tooltip == null) return;

        // Directly update tooltip text without restarting animations
        current.tooltip.SetText(values, content, header);
    }

}
