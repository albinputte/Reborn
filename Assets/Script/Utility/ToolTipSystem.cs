using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        if (targetRect != null)
        {
            Vector3[] corners = new Vector3[4];
            targetRect.GetWorldCorners(corners);
            // corners[1] = Top Left, corners[2] = Top Right
            current.tooltip.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f); // Bottom left

            Vector3 offset = new Vector3(-20f, -20f, 0f); // 10 pixels right, 10 pixels down
            current.tooltip.transform.position = corners[2] + offset;

        }
        else
        {
            current.tooltip.transform.position = Input.mousePosition;
        }

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
