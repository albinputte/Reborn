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
    private InventoryController inventoryController;
    public event Action<AccesoriesSlot> OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
    public UIToolTipTrigger uitipTrigger;


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
        Item.EquipAccesorie();
        ItemImage.enabled = true;
        ItemImage.sprite = Item.Icon;
        ItemFrame.sprite = FrameImage[1];
        IsEmpty = false;
        if (uitipTrigger != null)
        {
            uitipTrigger.header = item1.Name;
            uitipTrigger.content = item1.Description;
            BuffBase buffBase = item1.BuffBase;
            if (buffBase is AddetiveBuff buff)
            {
                uitipTrigger.values = buff.statType.ToString() + " +" + buff.bonusMultiplier;
            }
        }
    }

    public void RemoveAccesoires(bool ShouldAdd)
    {
        if(ShouldAdd)
            inventoryController.inventoryData.AddItem(Item, 1,null);
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
