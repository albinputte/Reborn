using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
 
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private InventoryUiSlot item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<InventoryUiSlot>();
        item.gameObject.SetActive(false);

    }

    

    public void SetData(Sprite sprite, int quantity)
    {

        item.gameObject.SetActive(true);
        item.SetData(sprite, quantity,"", "");

    }
    void Update()
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

    private void OnEnable()
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




    public void Toggle(bool value)
    {
        gameObject.SetActive(value);
    }
}
