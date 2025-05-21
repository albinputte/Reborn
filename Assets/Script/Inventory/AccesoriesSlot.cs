using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccesoriesSlot : MonoBehaviour
{
    public Image ItemImage;
    public Sprite[] FrameImage;
    [SerializeField] private Image ItemFrame; 
    public bool IsEmpty = true;
    private AccesoriesItemBase Item;
    private InventoryController inventoryController;
    

    public void Awake()
    {
        inventoryController = InventoryController.Instance;
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame = GetComponent<Image>();
    }
    public void SetAccesorie(AccesoriesItemBase item1)
    {
        Item = item1;
        ItemImage.enabled = true;
        ItemImage.sprite = Item.Icon;
        ItemFrame.sprite = FrameImage[1];
        IsEmpty = false;
    }

    public void RemoveAccesoires()
    {
        inventoryController.inventoryData.AddItem(Item, 1,null);
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame.sprite = FrameImage[0];
        IsEmpty = true;
    }


    public void OnPointerClick(BaseEventData data)
    {

        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
          

        }
        else
        {
            if(!IsEmpty)
                RemoveAccesoires();
        }
    }
}
