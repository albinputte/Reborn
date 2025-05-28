using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AccesorieSlotManger : MonoBehaviour
{
    public AccesoriesSlot[] Slots;
    public static AccesorieSlotManger Instance;
    [SerializeField] private InventoryUiPage InvUiPage;
    public void Awake()
    {
        Instance = this;
        InvUiPage = GetComponentInParent<InventoryUiPage>();
    }
    private void Start()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].OnItemBeginDrag +=  OnItemDrag;
            Slots[i].OnItemEndDrag += OnEndDrag; 
        }
    }


    public void SetSlot(AccesoriesItemBase Item)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].IsEmpty)
            {
                Slots[i].SetAccesorie(Item);
                return;
            }
        }
    }
    public ItemData GetItemAndRemove(int slot)
    {
        ItemData item = Slots[slot].GetItemFromSlot();
        Slots[slot].RemoveAccesoires(false);
        return item;
    }

    public void OnItemDrag(AccesoriesSlot slot)
    {
        int index = Array.IndexOf(Slots, slot);

        DragContext.SourceType = DragSourceType.AccesorieSlot;
        DragContext.SourceIndex = index;
        InvUiPage.SetMouse(Slots[index].GetItemFromSlot().Icon, 1);

    }

    public void OnEndDrag(AccesoriesSlot slot)
    {
        DragContext.SourceIndex = -1;
        InvUiPage.ResetMouse();
    }


}


