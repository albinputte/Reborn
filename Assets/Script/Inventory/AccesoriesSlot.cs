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
    public object OwnerPage { get; private set; }
    public event Action<AccesoriesSlot> OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
    public UIToolTipTrigger uitipTrigger;

    private ChestController chest; 
    public void Awake()
    {
        OwnerPage = this;
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        chest = FindAnyObjectByType<ChestController>();
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
        if (DragContext.SourceIndex == -1)
            return;

        AccesoriesItemBase incomingAccessory = null;

        // 1. Handle accessory dragged from another accessory slot
        if (DragContext.SourceType == DragSourceType.AccesorieSlot)
        {
            incomingAccessory = AccesorieSlotManger.Instance.GetItemAndRemove(DragContext.SourceIndex) as AccesoriesItemBase;
        }
        // 2. Handle accessory dragged from inventory
        else if (DragContext.SourceType == DragSourceType.Inventory)
        {
            InventoryItem invItem = InventoryController.Instance.inventoryData.GetSpecificItem(DragContext.SourceIndex);
            if (invItem.item is AccesoriesItemBase acc)
            {
                incomingAccessory = acc;
                InventoryController.Instance.inventoryData.RemoveItem(DragContext.SourceIndex, 1);
            }
        }
        // 3. Handle accessory dragged from chest
        else if (DragContext.SourceType == DragSourceType.Chest)
        {
            InventoryItem chestItem = chest.chestData.GetSpecificItem(DragContext.SourceIndex);
            if (chestItem.item is AccesoriesItemBase acc)
            {
                incomingAccessory = acc;
                chest.chestData.RemoveItem(DragContext.SourceIndex, 1);
            }
        }

        if (incomingAccessory == null)
            return;

        // 4. Drop logic
        if (IsEmpty)
        {
            SetAccesorie(incomingAccessory);
        }
        else
        {
            AccesoriesItemBase currentItem = GetItemFromSlot();
            RemoveAccesoires(false);
            SetAccesorie(incomingAccessory);

            // 5. Return the replaced accessory to the correct source
            switch (DragContext.SourceType)
            {
                case DragSourceType.Inventory:
                    InventoryController.Instance.inventoryData.AddItemToSpecificPos(currentItem, 1, null, DragContext.SourceIndex);
                    break;
                case DragSourceType.Chest:
                    chest.chestData.AddItemToSpecificPos(currentItem, 1, null, DragContext.SourceIndex);
                 
                    break;
                case DragSourceType.AccesorieSlot:
                    AccesorieSlotManger.Instance.Slots[DragContext.SourceIndex].SetAccesorie(currentItem);
                    break;
            }
        }
    }



    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);

        

    }
}
