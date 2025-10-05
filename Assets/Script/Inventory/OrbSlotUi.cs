using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrbSlotUi : MonoBehaviour
{
    public Image ItemImage;
    public Sprite[] FrameImage; // 0: empty, 1: occupied
    [SerializeField] private Image ItemFrame;
    public bool IsEmpty = true;
    private OrbsItemData Item;
    private InventoryController inventoryController;
    [SerializeField] private GameObject Player;
    public event Action<OrbSlotUi> OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;

    public void Awake()
    {
        inventoryController = InventoryController.Instance;
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame = GetComponent<Image>();
        SceneManger.instance.OnAllEssentialScenesLoaded += PrepareRefrences;
    }

    public void PrepareRefrences()
    {
        Player = GameObject.Find("Player");
    }
    public void SetOrb(OrbsItemData orbItem)
    {
        Item = orbItem;
        orbItem.PerformAction(Player, null);
        ItemImage.enabled = true;
        ItemImage.sprite = Item.Icon;
        ItemFrame.sprite = FrameImage[1];
        IsEmpty = false;
    }

    public void RemoveOrb(bool shouldAddToInventory, bool DeEquipOrb)
    {
        if (shouldAddToInventory)
        {
            InventoryController.Instance.inventoryData.AddItem(Item, 1, null);
       
        }
        if(DeEquipOrb)
            Item.RemoveOrb();

        ItemImage.sprite = null;
        ItemImage.enabled = false;
        ItemFrame.sprite = FrameImage[0];
        IsEmpty = true;
    }

    public OrbsItemData GetItemFromSlot()
    {
        return Item;
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            // Optional: Add right-click context menu or interaction
        }
        else
        {
            if (!IsEmpty)
                RemoveOrb(true, true);
        }
    }

    public void OnBeginDrag()
    {
        if (IsEmpty)
            return;

        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        if (DragContext.SourceIndex == -1)
            return;

        InventoryItem item = InventoryController.Instance.inventoryData.GetSpecificItem(DragContext.SourceIndex);

        if (item.item is OrbsItemData)
        {
            InventoryController.Instance.inventoryData.RemoveItem(DragContext.SourceIndex, 1);

            if (IsEmpty)
            {
                SetOrb(item.item as OrbsItemData);
            }
            else
            {
                InventoryController.Instance.inventoryData.AddItemToSpecificPos(Item, 1, null, DragContext.SourceIndex);
                RemoveOrb(false,true);
                SetOrb(item.item as OrbsItemData);
            }
        }
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }
}
