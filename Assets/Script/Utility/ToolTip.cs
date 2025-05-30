using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private TextMeshProUGUI valuesField;
    [SerializeField] public LayoutElement layoutElement;

    [SerializeField] private int characterWrapLimit;

    [SerializeField] RectTransform rectTransform;

    private Vector2 currentPivot;
    private bool isPivotLocked = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetText(string values, string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        if (string.IsNullOrEmpty(content))
        {
            contentField.gameObject.SetActive(false);
        }
        else
        {
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }


        if (string.IsNullOrEmpty(values))
        {
            valuesField.gameObject.SetActive(false);
        }
        else
        {
            valuesField.gameObject.SetActive(true);
            valuesField.text = values;
        }

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        int valuesLength = valuesField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit || valuesLength > characterWrapLimit) ? true : false;
        UnlockPivot();
        UpdatePivotAndPosition();
    }
    private void Update()
    {

    }
    private void UpdatePivotAndPosition()
    {
        Vector2 position = Input.mousePosition;

        // Calculate the new pivot based on mouse position
        var normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
        currentPivot = CalculatePivot(normalizedPosition);
        rectTransform.pivot = currentPivot;

        // Set the tooltip position to follow the mouse
        transform.position = position;
    }

    private Vector2 CalculatePivot(Vector2 normalizedPosition)
    {
        // Adjusted pivots to bring tooltip closer with rounded corners
        var pivotTopLeft = new Vector2(0.05f, 0.95f);
        var pivotTopRight = new Vector2(0.95f, 0.95f);
        var pivotBottomLeft = new Vector2(0.05f, 0.05f);
        var pivotBottomRight = new Vector2(0.95f, 0.05f);

        if (normalizedPosition.x < 0.5f && normalizedPosition.y <= 0.5f)
        {
            return pivotTopLeft;
        }
        else if (normalizedPosition.x > 0.5f && normalizedPosition.y >= 0.5f)
        {
            return pivotTopRight;
        }
        else if (normalizedPosition.x <= 0.5f && normalizedPosition.y < 0.5f)
        {
            return pivotTopLeft;
        }
        else
        {
            return pivotTopRight;
        }
    }

    public void LockPivot()
    {
        isPivotLocked = true;
    }

    public void UnlockPivot()
    {
        isPivotLocked = false;
    }


}
