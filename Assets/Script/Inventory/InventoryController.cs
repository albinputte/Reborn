using JetBrains.Annotations;
using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;


public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryUiPage inventoryUi;
    public ItemData TrashItem;
    [SerializeField]
    public InventorySO inventoryData;
    public static bool NoWeaponEquiped;
    public static bool IsConsumableEquiped;
    public static bool IsToolEquiped;
    public static bool IsAccesoireInHand;
 
 
    public static InventoryController Instance;
    public ChestController Chest;

    [SerializeField] 
    private GameObject itemPrefab; //For Dropping item

    [SerializeField]
    public bool InventoryUiActive;

    [SerializeField]
    private PlayerInputManger inputManger;

    [SerializeField]
    private CraftingUI craftingUI; //temporary

    private int CurrentIndex;
   
    public ItemData testItem;

    public GameObject Character;

    public List<InventoryItem> ItemToInitialize  = new List<InventoryItem>();

    private bool BuildItemEquiped;
    private StructureItemBase BuildItem;
    private bool CanEat = true;
 
    private void Start()
    {
        PrepareInventoryData();
        PrepareInventoryUI();
       


    }
    private void Awake()
    {
        Instance = this;
        InventoryController.NoWeaponEquiped = true;
    }

    private void PrepareInventoryUI()
    {
        inventoryUi.ShowInventory(); 
        inventoryUi.InstantiateInventory();
        inventoryUi.OnSwap += HandleItemSwap;
        inventoryUi.OnDrag += HandleDragging;
        inventoryUi.OnItemAction += HandleItemSelection;
        inventoryUi.OnDropItem += HandleDropIitem;
        inventoryUi.OnHotbarAction += HandleHotbarAction;
        inventoryUi.HideInventory();
    }

    private void PrepareInventoryData()
    {
        inventoryData.InstantiateInventory();
        inventoryData.OnInventoryChange += UpdateInventoryUI;
      
        for (int i = inventoryData.Inventory.Count -10; i <= inventoryData.Inventory.Count -1; i++)
        {
            inventoryData.AddItemToSpecificPos(TrashItem, 1, null, i);
        }
        foreach (var item in ItemToInitialize)
        {
            if(item.IsEmpty)
                continue;
            Debug.Log(item.ToString());
            inventoryData.AddItem(item);
         
        }
     
        InstantiateWeaponInstances();
    }

    public void InstantiateWeaponInstances()
    {
        //move this to so file instead

      for (int i = 0;i < inventoryData.Inventory.Count; i++)
        {
            InventoryItem item1 = inventoryData.GetSpecificItem(i);
            if (!item1.IsEmpty)
            {
                if (item1.item.itemType == ItemType.Weapon)
                {
                    Debug.Log("yAY SWORD");
                    InventoryItem item = inventoryData.GetSpecificItem(i);
                    WeaponInstances instances = CreateWeaponIntances(item.item, null, 1000 + i);
                 
                    Debug.Log(instances.Weapon.Name);
                    inventoryData.addWeaponinstance(i, instances);
                }
            }
         
        }
    }

    public WeaponInstances CreateWeaponIntances(ItemData item, OrbsItemData orb, int index)
    {
        if (item is WeaponItemData weaponData)
            return new WeaponInstances(weaponData, orb, index);

        Debug.LogWarning("Tried to create weapon instance with non-weapon item");
        return null;
    }


    private void UpdateInventoryUI(Dictionary<int, InventoryItem> dictionary)
    {

        inventoryUi.ResetInventory();
        foreach(var item in dictionary)
        {
            inventoryUi.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description);
        }
        
       
        
     
    }

    private void HandleDragging(int index)
    {
        
        InventoryItem item = inventoryData.GetSpecificItem(index);
        if (item.IsEmpty)
            return;

        inventoryUi.SetMouse(item.item.Icon,item.quantity);
    }

    private void HandleItemSwap(InventoryUiSlot slot)
    {

        if (DragContext.SourceType == DragSourceType.Chest && slot.OwnerPage is InventoryUiPage)
        {
            Chest.chestData.MoveItemToInventory(slot.SlotIndex, DragContext.SourceIndex);
            SoundManager.PlaySound(SoundType.SwapItem_Inventory);

        }
        else if (DragContext.SourceType == DragSourceType.Inventory && slot.OwnerPage is InventoryUiPage
        )
        {
               
            InventoryItem item = inventoryData.GetSpecificItem(DragContext.SourceIndex);
            IOrb orb = item.item as IOrb;
            if (orb != null)
            {
                InventoryItem item2 = inventoryData.GetSpecificItem(slot.SlotIndex);
                IWeapon weapon = item2.item as IWeapon;
                if (weapon != null)
                {
                    OrbsItemData orbData = item.item as OrbsItemData;
                    item2.weaponInstances.UpdateOrb(orbData);
                    inventoryData.RemoveItem(DragContext.SourceIndex, 1);
                    return;
                }
            }
            if(DragContext.SourceIndex != slot.SlotIndex)
                SoundManager.PlaySound(SoundType.SwapItem_Inventory);
            inventoryData.SwapitemPlace(DragContext.SourceIndex, slot.SlotIndex);
        }
        else if (DragContext.SourceType == DragSourceType.AccesorieSlot)
        {
            SoundManager.PlaySound(SoundType.SwapItem_Inventory);
            InventoryItem item = inventoryData.GetSpecificItem(slot.SlotIndex);
            ItemData item2 = AccesorieSlotManger.Instance.GetItemAndRemove(DragContext.SourceIndex);
            if (item.item is AccesoriesItemBase)
            {
                inventoryData.RemoveItem(slot.SlotIndex, 1);
                AccesorieSlotManger.Instance.Slots[DragContext.SourceIndex].SetAccesorie(item.item as AccesoriesItemBase);
                inventoryData.AddItemToSpecificPos(item2, 1, null, slot.SlotIndex);

                return;
            }
            else
            {
                
                inventoryData.AddItemToSpecificPos(item2, 1, null, slot.SlotIndex);
                return;
            }

        }
        else if (DragContext.SourceType == DragSourceType.OrbSlot) 
        {
            SoundManager.PlaySound(SoundType.SwapItem_Inventory);
            InventoryItem item = inventoryData.GetSpecificItem(slot.SlotIndex);
            ItemData item1 = InventorySlideInUi.Instance.GetItemAndRemove(DragContext.SourceIndex);
            if (item.item is OrbsItemData) {
                inventoryData.RemoveItem(slot.SlotIndex, 1);
                InventorySlideInUi.Instance.Slot[DragContext.SourceIndex].SetOrb(item.item as OrbsItemData);
                inventoryData.AddItemToSpecificPos(item1, 1, null, slot.SlotIndex);
                return;
            
            }
            else
            {
                inventoryData.AddItemToSpecificPos(item1, 1, null, slot.SlotIndex);
                return;
            }

        }

    }

    private void HandleItemSelection(int index1)
    {
        InventoryItem item = inventoryData.GetSpecificItem(index1);
        CurrentIndex = index1;
        if (item.IsEmpty)
            return;
        if(InventoryController.IsAccesoireInHand)
        {
            OnAccesoires();
            return;
        }
        IWeapon weapon = item.item as IWeapon;
        if (weapon != null)
        {
            InventorySlideInUi.Instance.OpenSlider();
        }
        else {
            InventorySlideInUi.Instance.CloseSlider();
        }
        IDestroyableItem iDestroyableItem = item.item as IDestroyableItem;
        if (iDestroyableItem != null)
        {
            inventoryData.RemoveItem(index1, 1);
        }

        IitemAction iitemAction = item.item as IitemAction;
        if (iitemAction != null)
        {
            iitemAction.PerformAction(Character, item.weaponInstances);
        }
     
        
    }

    public void HandleHotbarAction(int index)
    {
        InventoryItem item = inventoryData.GetSpecificItem(index);
        CurrentIndex = index;

        InventoryController.NoWeaponEquiped = true;
        InventoryController.IsConsumableEquiped = false;
        if (InventoryController.IsToolEquiped)
        {
            StatSystem.instance.RemoveStats(StatsType.PickaxePower, StatSystem.instance.GetStat(StatsType.PickaxePower));
        }
        InventoryController.IsToolEquiped = false;
        InventoryController.IsAccesoireInHand = false;
        if (BuildItemEquiped)
        {
            BuildItem.CancelBuild(0);
        }
        BuildItemEquiped = false;
      
        if (item.IsEmpty || item.item == null)
        {
            if (InventoryUiActive)
                InventorySlideInUi.Instance.CloseSlider();
            return;
        }
         
      
        if (item.item is IWeapon weapon)
        {
            InventoryController.NoWeaponEquiped = false;
            if (item.item is IitemAction action)
            {
                action.PerformAction(Character, item.weaponInstances);
                if (InventoryUiActive)
                    InventorySlideInUi.Instance.OpenSlider();
               
            }
            return;
        }
        if(InventoryUiActive)
            InventorySlideInUi.Instance.CloseSlider();
        if (item.item is IStructure structure) {
            BuildItemEquiped = true;
            BuildItem = item.item as StructureItemBase;
            structure.PerformBuild(index);
    
        }
        if (item.item is IAccesories accesories)
        {
            InventoryController.IsAccesoireInHand = true;
         
        }

            if (item.item is IConsumable)
        {
            InventoryController.IsConsumableEquiped = true;
            return;
        }

        if (item.item is ITools tool)
        {
            InventoryController.IsToolEquiped = true;
            PickAxeItemBase pickAxe = (PickAxeItemBase)item.item;
            StatSystem.instance.AddStats(StatsType.PickaxePower, pickAxe.PickAxePower);
            return;
         
        }
    }

    public void OnAccesoires()
    {
        InventoryItem item = inventoryData.GetSpecificItem(CurrentIndex);
        if (item.item is IAccesories accesories)
        {
            //accesories.EquipAccesorie();
            inventoryUi.SetAccesoire(item.item as AccesoriesItemBase);
            inventoryData.RemoveItem(CurrentIndex, 1);
        }
    }

    public void HandleConsumable()
    {
        InventoryItem item = inventoryData.GetSpecificItem(CurrentIndex);
        if (item.item is IConsumable && CanEat)
        {
            SoundManager.PlaySound(SoundType.Eat_sound);
            StartCoroutine(EatCooldown());
            inventoryUi.EatCooldownTimer(CurrentIndex, 0.5f);
            if (item.item is IitemAction action)
                action.PerformAction(Character, item.weaponInstances);
            if (item.item is IDestroyableItem)
                inventoryData.RemoveItem(CurrentIndex, 1);
           
        }
        if (item.IsEmpty || item.quantity == 0)
        {
            InventoryController.IsConsumableEquiped = false;
        }
    }
    public IEnumerator EatCooldown()
    {
        if (CanEat)
        {
            CanEat = false;
            yield return new WaitForSeconds(0.5f);
            CanEat = true;
        }
    }



    public void HandleDropIitem(int index)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - Character.transform.position).normalized;

        InventoryItem item = inventoryData.GetSpecificItem(index);
        if(item.IsEmpty)
            return;
        inventoryData.RemoveItem(index, item.quantity);
        GameObject obj = Instantiate(itemPrefab, Character.transform.position + direction * 0.5f, Quaternion.identity); // spawn a little offset from character
        SoundManager.PlaySound(SoundType.ItemDrop);
        obj.GetComponent<WorldItem>().SetItem(item.item, item.quantity);
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 10;
        rb.AddForce(direction * 10, ForceMode2D.Impulse);

    }




    // Update is called once per frame
    public void InventoryInput()
    {

        if (!InventoryUiActive)
        {
            inventoryUi.ShowInventory();
            InventoryUiActive = true;
            
            foreach (var item in inventoryData.GetInventoryState())
            {
               
                inventoryUi.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description);

            }
            if (TutorialManger.instance.TutorialIsActive && TutorialManger.instance.currentState == TutorialState.PickedUpSword)
            {
                TutorialManger.instance.OnInventoryOpened();
            }
            TransferHotbarToInventory();
           
        }
        else if (InventoryUiActive)
        {

            TransferInventoryToHotbar();
            inventoryUi.HideInventory();
            InventoryUiActive = false;

        }



    }
    public void HandleItemSwap(int index, int index1) => inventoryData.SwapitemPlace(index, index1);

    public void ScrollInHotbar(int index)
    {
        inventoryUi.SelectHotBarSlotScroll(index);
    }
    public void TransferHotbarToInventory()
    {
        int count = inventoryData.Inventory.Count - 10;
        for (int i = 0; i < 10; i++)
        {
            HandleItemSwap(count + i, i);
        }
        inventoryUi.SwitchHotbarSlot();
        inventoryUi.HideHotbar();
    }

    public void TransferInventoryToHotbar()
    {
        int count = inventoryData.Inventory.Count - 10;
        for (int i = 0; i < 10; i++)
        {
            HandleItemSwap(count + i, i);
        }
        inventoryUi.SwitchHotbarSlot();
        inventoryUi.ShowHotbar();
    }

    public bool IsWeapon(int index)
    {
        InventoryItem item = inventoryData.GetSpecificItem(index);
        IWeapon weapon = item.item as IWeapon;
        if (weapon != null)
            return true;
        return false;

    }
    public void SetCurrentHotbarIndex(int index)
    {
        inputManger.HotbarIndex = index;
    }

    public  WeaponInstances GetWeaponInstances(int index)
    {
        return inventoryData.GetSpecificItem(index).weaponInstances;
    }
    public void TransferItemToChest(int inventoryIndex, int chestIndex)
    {
        InventoryItem item = inventoryData.GetSpecificItem(inventoryIndex);
        if (item.IsEmpty) return;

        int remainingQuantity = Chest.chestData.AddItem(item.item, item.quantity, item.weaponInstances);
        if (remainingQuantity < item.quantity)
        {
            inventoryData.RemoveItem(inventoryIndex, item.quantity - remainingQuantity);
           
        }
    }



}
