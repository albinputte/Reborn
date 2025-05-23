using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestUiPage : MonoBehaviour
{
    [SerializeField]
    private InventoryUiSlot[] chestSlots;
    private List<InventoryUiSlot> chestSlotList = new List<InventoryUiSlot>();

    public GameObject chestPanel;
    private InventoryUiSlot selectedChestSlot;

    [SerializeField] private MouseFollower mouseFollower;

    private int currentDraggingIndex;
    public Action<int> OnDrag, OnItemAction;
    public Action<InventoryUiSlot> OnSwap;
    private InventoryUiSlot selectedSlot;
    private bool IsSelected;

    public void InstantiateChest()
    {
        chestSlots = GetComponentsInChildren<InventoryUiSlot>();
        chestSlotList = chestSlots.OrderBy(slot => ExtractNumberFromName(slot.name)).ToList();

        foreach (InventoryUiSlot slot in chestSlotList)
        {
            slot.OnItemClicked += ItemSelection;
            slot.OnItemBeginDrag += BeginDrag;
            slot.OnItemDroppedOn += ItemSwap;
            slot.OnItemEndDrag += EndDrag;
            slot.OnRightMouseBtnClick += ShowItemActions;
            slot.Init(this, ExtractNumberFromName(slot.name));
            //slot.OnItemSelect += SelectionBorder;

        }
        HideChest();
    }

    public void UpdateChestData(int Index, Sprite newSprite, int newQuantity, string itemname, string itemDesciption)
    {
        if (chestSlotList.Count >= Index)
        {
            Debug.Log("added " +  Index);
            chestSlotList[Index].SetData(newSprite, newQuantity, itemname, itemDesciption);
        }
        else
        {
            Debug.Log("Mikael wtf did you do");
        }
    }
    public void ResetChest()
    {
        foreach (var item in chestSlotList)
        {
            item.ResetItemData();
            item.DeselectBorder();
        }
    }



    private void ItemSelection(InventoryUiSlot slot)
    {
        int index = chestSlotList.IndexOf(slot);
        // Handle item selection logic
    }

    private void ShowItemActions(InventoryUiSlot slot)
    {
        IsSelected = slot.IsSelected;
        int index = chestSlotList.IndexOf(slot);
        if (index == -1)
            return;

        if (!IsSelected)
        {
            if (selectedChestSlot == null)
                selectedChestSlot = slot;
            selectedChestSlot.DeselectBorder();
            slot.SelectBorder();
            selectedChestSlot = slot;
            slot.IsSelected = true;

        }
        else
        {
            selectedChestSlot.IsSelected = false;
            selectedChestSlot.DeselectBorder();
            OnItemAction?.Invoke(index);

        }

    }

    private void ItemSwap(InventoryUiSlot slot)
    {
       
        OnSwap?.Invoke(slot);
    }

    private void BeginDrag(InventoryUiSlot slot)
    {
        int index = chestSlotList.IndexOf(slot);
        if (index == -1)
            return;

        DragContext.SourceType = DragSourceType.Chest;
        DragContext.SourceIndex = index;
        OnDrag?.Invoke(DragContext.SourceIndex);
    }

    private void EndDrag(InventoryUiSlot slot)
    {
        int index = chestSlotList.IndexOf(slot);
        ResetMouse();
        //if (!IsPointerOverUI())
            //OnDropItem?.Invoke(index);

    }
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void ResetMouse()
    {
        currentDraggingIndex = -1;
        mouseFollower.Toggle(false);
    }
    public void SetMouse(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    public void ShowChest()
    {
        chestPanel.SetActive(true);
        SoundManager.PlaySound(SoundType.ChestOpen);
 
    }

    public void HideChest()
    {
        chestPanel.SetActive(false);
        SoundManager.PlaySound(SoundType.ChestClose);

    }

    private int ExtractNumberFromName(string name)
    {

        int number = 0;
        string numStr = new string(name.Where(char.IsDigit).ToArray());
        int.TryParse(numStr, out number);
        return number;
    }


}
