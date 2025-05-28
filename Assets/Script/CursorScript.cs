using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite[] cursorSprites; // 0: default, 1: weapon equipped

    private RectTransform rectTransform;
    private int currentCursorIndex = -1;

    private void Awake()
    {
        Cursor.visible = false;

        if (cursorImage == null)
            cursorImage = GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Move cursor
        rectTransform.position = Input.mousePosition;

        // Determine which sprite to show
        int targetIndex = InventoryController.NoWeaponEquiped ? 0 : 1;

        if (targetIndex != currentCursorIndex && targetIndex < cursorSprites.Length)
        {
            cursorImage.sprite = cursorSprites[targetIndex];
            currentCursorIndex = targetIndex;
        }
    }
}
