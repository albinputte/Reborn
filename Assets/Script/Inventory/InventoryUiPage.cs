using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUiPage : MonoBehaviour
{
    [SerializeField]
    private InventoryUiSlot[] itemSlot;

    List<InventoryUiSlot> ListOfUIslots = new List<InventoryUiSlot>();

    List<InventoryUiSlot> ListOfUIAccesoriesSlots = new List<InventoryUiSlot>();

    [SerializeField] private MouseFollower mouseFollower;


    private int currentDraggingIndex;

    public Action<int> OnDrag;

    public Action<int, int> OnSwap;


    private void Start()
    {
   
        mouseFollower.Toggle(false);
        currentDraggingIndex = -1;
    }

    public void InstantiateInventory()
    {
        itemSlot = FindObjectsOfType<InventoryUiSlot>(); //need to find naother way to find the slots bcus multiole other slots are being found

        ListOfUIslots = itemSlot.OrderBy(slot => ExtractNumberFromName(slot.name)).ToList();
        Debug.Log(ListOfUIslots.Count);
        foreach (InventoryUiSlot slot in ListOfUIslots)
        {
            slot.OnItemClicked += ItemSelection;
            slot.OnItemBeginDrag += BeginDrag;
            slot.OnItemDroppedOn += ItemSwap;
            slot.OnItemEndDrag += EndDrag;
            slot.OnRightMouseBtnClick += ShowItemActions;
          
        }

        
    


    }

    public void UpdateData(int Index, Sprite newSprite, int newQuantity)
    {
        if(ListOfUIslots.Count > Index)
        {
            ListOfUIslots[Index].SetData(newSprite, newQuantity);
        }
        else
        {
            Debug.Log("Mikael wtf did you do");
        }
    }

    private void ShowItemActions(InventoryUiSlot slot)
    {
        
    }

    private void EndDrag(InventoryUiSlot slot)
    {
        ResetMouse();
    }

    private void ItemSwap(InventoryUiSlot slot)
    {
        int index = ListOfUIslots.IndexOf(slot);
        if (index == -1 || currentDraggingIndex == -1)
            return;
        OnSwap?.Invoke(currentDraggingIndex, index);
       

    }

    private void BeginDrag(InventoryUiSlot slot)
    {
       int index = ListOfUIslots.IndexOf(slot);
        if (index == -1) 
            return;
       
       currentDraggingIndex = index;
        Debug.Log(currentDraggingIndex);
       OnDrag?.Invoke(currentDraggingIndex);
    }

   public void ResetMouse()
    {
        currentDraggingIndex = -1;
        mouseFollower.Toggle(false);
    }

    public void ResetInventory()
    {
        foreach(var item in ListOfUIslots)
        {
            item.ResetItemData();
            item.DeselectBorder();
        }
    }
    public void SetMouse(Sprite sprite, int quantity)
    {
        mouseFollower.SetData(sprite, quantity);
        mouseFollower.Toggle(true);
    }

    private void ItemSelection(InventoryUiSlot slot)
    {
       
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
    }
    public void HideInventory() 
    {
        gameObject.SetActive(false); 
    }

    private int ExtractNumberFromName(string name)
    {
        
        int number = 0;
        string numStr = new string(name.Where(char.IsDigit).ToArray());
        int.TryParse(numStr, out number);
        return number;
    }


}
