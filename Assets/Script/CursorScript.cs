using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public Image CursorImage;
    public Sprite[] CursorSprite;
    private Transform transform;
    void Awake()
    {
        Cursor.visible = false;
        CursorImage = GetComponent<Image>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        if(InventoryController.NoWeaponEquiped)
        {
            if (CursorImage.sprite != CursorSprite[0])
                CursorImage.sprite = CursorSprite[0];
        }

        else
        {
            if (CursorImage.sprite != CursorSprite[1])
                CursorImage.sprite = CursorSprite[1];
        }
    }
          
}
