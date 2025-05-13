using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public static ItemToolTip Instance;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text nameText;
    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        Instance = this;
        HideTooltip(); // Hide on start
    }

    public void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
                );
        transform.position = canvas.transform.TransformPoint(position);

    }

    public void ShowTooltip(string itemName, string description)
    {
        Description.text = description;
        nameText.text = itemName;
        tooltipPanel.SetActive(true);


    }

    public void HideTooltip()
    {
       Instance.tooltipPanel.SetActive(false);
    }
}