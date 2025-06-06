using System;
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
  
    public event Action<AccesoriesSlot> OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
    public UIToolTipTrigger uitipTrigger;


    public void Awake()
    {
        
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame = GetComponent<Image>();
        uitipTrigger = GetComponent<UIToolTipTrigger>();
    }
    public void SetAccesorie(AccesoriesItemBase item1)
    {
        Item = item1;
        Item.EquipAccesorie();
        ItemImage.enabled = true;
        ItemImage.sprite = Item.Icon;
        ItemFrame.sprite = FrameImage[1];
        IsEmpty = false;
        if (uitipTrigger != null)
        {
            uitipTrigger.header = item1.Name;
            uitipTrigger.content = item1.Description;
            InventoryItem item = new InventoryItem();
            item.item = item1;
            uitipTrigger.values = InventoryController.Instance.GetStatsForTooltip(item);
        }
    }

    public void RemoveAccesoires(bool ShouldAdd)
    {
        if(ShouldAdd)
            InventoryController.Instance.inventoryData.AddItem(Item, 1,null);
        Item.RemoveAccesorie();
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame.sprite = FrameImage[0];
        IsEmpty = true;
    }

    public AccesoriesItemBase GetItemFromSlot()
    {
        return Item;
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
                RemoveAccesoires(true);
        }
    }

    public void OnBeginDrag()
    {

        if (IsEmpty)
        {
            return;
        }
        //uitipTrigger.Dragging();
       
        OnItemBeginDrag?.Invoke(this); 

    }

    public void OnDrop()
    {
        if(DragContext.SourceIndex == -1)
            return;
        InventoryItem item = InventoryController.Instance.inventoryData.GetSpecificItem(DragContext.SourceIndex);
        
        if(item.item is AccesoriesItemBase)
        {
            InventoryController.Instance.inventoryData.RemoveItem(DragContext.SourceIndex, 1);
            if (IsEmpty)
            {
                SetAccesorie(item.item as AccesoriesItemBase);

            }
            else
            {
   
                InventoryController.Instance.inventoryData.AddItemToSpecificPos(Item, 1, null, DragContext.SourceIndex);
                RemoveAccesoires(false);
                SetAccesorie(item.item as AccesoriesItemBase);
          

            }
            

        }
   
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);

        

    }
}
