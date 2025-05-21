using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class InventoryUiPage : MonoBehaviour
{
    [SerializeField]
    private InventoryUiSlot[] itemSlot;
    [SerializeField]
    private InventoryUiSlot[] HotBarSlots;
    [SerializeField]
    private InventoryUiSlot[] HiddenHotbarSlots;
    [SerializeField]
    private AccesorieSlotManger accesorieSlots;

   List<InventoryUiSlot> ListOfUIslots = new List<InventoryUiSlot>();
    public GameObject Hotbar;
    [SerializeField] private InventorySlideInUi SlideInUi;

    List<InventoryUiSlot> ListOfUIAccesoriesSlots = new List<InventoryUiSlot>();

    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private LayerMask UILayer;
   

    private int currentDraggingIndex;
    private bool IsSelected;
    public Action<int> OnDrag, OnItemAction;
    private InventoryUiSlot Selectdslot;
    public Action<InventoryUiSlot> OnSwap;
    public Action<int> OnDropItem, OnHotbarAction;
    private int ScrollIndex;


    private void Awake()
    {
        mouseFollower.Toggle(false);
        currentDraggingIndex = -1;
        ScrollIndex = -1;
    }

   

    public void InstantiateInventory()
    {
        AddNewSlots();
        SlideInUi.ResetSlideIcons();
        ListOfUIslots = itemSlot.OrderBy(slot => ExtractNumberFromName(slot.name)).ToList();
        foreach (InventoryUiSlot slot in ListOfUIslots)
        {
            slot.OnItemClicked += ItemSelection;
            slot.OnItemBeginDrag += BeginDrag;
            slot.OnItemDroppedOn += ItemSwap;
            slot.OnItemEndDrag += EndDrag;
            slot.OnRightMouseBtnClick += ShowItemActions;
            slot.OnItemSelect += SelectionBorder;
            slot.Init(this, ExtractNumberFromName(slot.name));


        }

    }


    private void AddNewSlots()
    {
        InventoryUiSlot[] existingSlots = HotBarSlots;
        InventoryUiSlot[] newSlots = GetComponentsInChildren<InventoryUiSlot>();

     
        var combinedSlots = existingSlots
            .Concat(newSlots.Where(slot => !existingSlots.Contains(slot)))
            .ToArray();

        itemSlot = combinedSlots;
    }

    public void SetSlideIn(WeaponInstances inst)
    {
        SlideInUi.SetSlideIcons(inst.Weapon.Icon, null);
        
    }

    public void UpdateData(int Index, Sprite newSprite, int newQuantity,string itemname, string itemDesciption)
    {
        if(ListOfUIslots.Count > Index)
        {
           
            ListOfUIslots[Index].SetData(newSprite, newQuantity, itemname, itemDesciption);
        }
        else
        {
            Debug.Log("Mikael wtf did you do");
        }
      
    }

    public void SetAccesoire(AccesoriesItemBase accesories)
    {
        accesorieSlots.SetSlot(accesories);
    }

    private void ShowItemActions(InventoryUiSlot slot)
    {
        IsSelected = slot.IsSelected;
        int index = ListOfUIslots.IndexOf(slot);
        if (index == -1)
            return;
     
        InventoryController.Instance.SetCurrentHotbarIndex(correctHotbarIndex(index));
      

      
        if (!IsSelected)
        {
            if (Selectdslot == null)
                Selectdslot = slot;
            Selectdslot.DeselectBorder();
            slot.SelectBorder();
            OnHotbarAction?.Invoke(index);
            
            Selectdslot = slot;
            slot.IsSelected = true;
            
        }
        else 
        {
            Selectdslot.IsSelected = false;
            Selectdslot.DeselectBorder();
            OnItemAction?.Invoke(index); 
     
        }
      
    }

    public int correctHotbarIndex(int index)
    {
        int totalSlots = ListOfUIslots.Count;
        int lastTenStart = totalSlots - 10;

        if (index >= 0 && index <= 9)
        {
            return index + 1;
        }
        else if (index == 10 || index == totalSlots)
        {
            return 0;
        }
        else if (index >= lastTenStart && index < totalSlots)
        {
            return (index - lastTenStart) + 1 ;
        }

        return ScrollIndex;
    }



    public void SelectHotBarSlotScroll(int index)
    {
     
        if(Selectdslot != null)
            Selectdslot.DeselectBorder();
        Selectdslot = HotBarSlots[index];
        HotBarSlots[index].SelectBorder();
        ScrollIndex = index;
        HotBarSlots[index].IsSelected = true;
        SetItemOnHotbar(HotBarSlots[index]);
    }

    public void SetItemOnHotbar(InventoryUiSlot slot)
    {
        int index = ListOfUIslots.IndexOf(slot);
        OnHotbarAction?.Invoke(index);
    }

    private void EndDrag(InventoryUiSlot slot)
    {
        int index = ListOfUIslots.IndexOf(slot);
        ResetMouse();
        if(!IsPointerOverUI())
            OnDropItem?.Invoke(index);
       
    }
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    private void ItemSwap(InventoryUiSlot slot)
    {

        OnSwap?.Invoke(slot);
       

    }

    private void BeginDrag(InventoryUiSlot slot)
    {
       int index = ListOfUIslots.IndexOf(slot);
        if (index == -1) 
            return;

        DragContext.SourceType = DragSourceType.Inventory;
        DragContext.SourceIndex = index;
        OnDrag?.Invoke(DragContext.SourceIndex);
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
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void ItemSelection(InventoryUiSlot slot)
    {
        //slot.SelectBorder();  
    }

    public void SelectionBorder(InventoryUiSlot slot)
    {
   
        int index = ListOfUIslots.IndexOf(slot);
        if (!InventoryController.Instance.IsWeapon(index))
        {
            SlideInUi.ResetSlideIcons();
            return;
        }
        
           
        WeaponInstances weaponInst = InventoryController.Instance.GetWeaponInstances(index);
        OrbsItemData orb = weaponInst.GetOrb();
        if (orb != null)
        {
            SlideInUi.SetSlideIcons(weaponInst.Weapon.Icon, orb.Icon);
        }
        else
        {
            SlideInUi.SetSlideIcons(weaponInst.Weapon.Icon, null);
        }

    }

    public void SwitchHotbarSlot()
    {
        var Hotbar = HotBarSlots;
        HotBarSlots = HiddenHotbarSlots;
        HiddenHotbarSlots = Hotbar;
        if(ScrollIndex != -1)
            SelectHotBarSlotScroll(ScrollIndex);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
      
    }
    public void HideInventory() 
    {
        gameObject.SetActive(false);
        
    }

    public void ShowHotbar()
    {
        Hotbar.SetActive(true);
    }

    public void HideHotbar()
    {
        Hotbar.SetActive(false);
    }

    private int ExtractNumberFromName(string name)
    {
        
        int number = 0;
        string numStr = new string(name.Where(char.IsDigit).ToArray());
        int.TryParse(numStr, out number);
        return number;
    }



 


}
