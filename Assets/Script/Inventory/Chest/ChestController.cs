using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{

    [SerializeField] private ChestUiPage chestUi;
    [SerializeField] public ChestSO chestData;
    [SerializeField] private GameObject itemPrefab; // For dropping items
    public bool ChestUiIsActive;
    private int currentIndex;
    [SerializeField] InventorySO inventory;

   

    public void Start()
    {
        InitializeChest();
        HideChest();
    }

    public void InitializeChest()
    {
    
        chestUi.InstantiateChest();
        chestUi.OnSwap += HandleItemSwap;
        chestUi.OnDrag += HandleDragging;
        chestUi.OnItemAction += HandleItemAction;
    }

    private void HandleDragging(int index)
    {
        InventoryItem item = chestData.GetSpecificItem(index);
        if (item.IsEmpty)
            return;

        chestUi.SetMouse(item.item.Icon, item.quantity);
    }
    public void PrepareChestData(List<InventoryItem> ItemToInitialize)
    {
        chestData.InstantiateInventory();
        chestData.OnInventoryChange += UpdateChestUI;
        foreach (var item in ItemToInitialize)
        {
            if (item.IsEmpty)
                continue;
            Debug.Log(item.ToString());
            chestData.AddItem(item);

        }
     
    }

    private void HandleItemSwap(InventoryUiSlot slot)
    {
        Debug.Log(slot.name.ToString());

        if (DragContext.SourceType == DragSourceType.Inventory && slot.OwnerPage is ChestUiPage)
        {
            InventoryController.Instance.inventoryData.MoveItemToChest(chestData, slot.SlotIndex, DragContext.SourceIndex);
        }
        else if (DragContext.SourceType == DragSourceType.Chest && slot.OwnerPage is ChestUiPage
        )
        {
            chestData.SwapitemPlace(DragContext.SourceIndex, slot.SlotIndex);
        }
    }

    private void HandleItemAction(int index)
    {
        InventoryItem item = chestData.GetSpecificItem(index);
        currentIndex = index;

        if (item.IsEmpty) return;
        TransferItemFromChest(index);

    }

    public void ShowChest()
    {
        chestUi.ShowChest();
        ChestUiIsActive = true;
        UpdateChestUI();
    }

    public void HideChest()
    {
        chestUi.HideChest();
        ChestUiIsActive = false;

    }

    public void UpdateChestUI()
    {
        chestUi.ResetChest();

        foreach (var item in chestData.GetChestState())
        {
            chestUi.UpdateChestData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description);
        }
    }

    private void UpdateChestUI(Dictionary<int, InventoryItem> dictionary)
    {

        chestUi.ResetChest();
        foreach (var item in dictionary)
        {
            chestUi.UpdateChestData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description);
        }




    }

    public void TransferItemFromChest(int chestIndex)
    {
        InventoryItem item = chestData.GetSpecificItem(chestIndex);
        if (item.IsEmpty) return;

        inventory.AddItem(item.item, item.quantity, item.weaponInstances);
        chestData.RemoveItem(chestIndex, item.quantity);
        
    }
}

