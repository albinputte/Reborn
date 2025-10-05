using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private ChestUiPage chestUi;
    [SerializeField] public ChestSO chestData;
    [SerializeField] private GameObject itemPrefab; // For dropping items
    public bool ChestUiIsActive;
    private int currentIndex;
    [SerializeField] InventorySO inventory;
    public string ChestName;

    private void Start()
    {
        Debug.Log("ChestController Start");
        InitializeChest();
    }

    private void OnDisable()
    {
        Debug.Log("ChestController OnDisable");
        CleanupListeners();
    }

    private void OnDestroy()
    {
        Debug.Log("ChestController OnDestroy");
        CleanupListeners(); // <- This ensures listeners are always cleared on scene reload
    }

    public void InitializeChest()
    {
        Debug.Log("Initializing Chest");
        ChestUiIsActive = false;


        CleanupListeners(); // Prevent double subscriptions

        if (chestUi != null)
        {
            chestUi.OnSwap += HandleItemSwap;
            chestUi.OnDrag += HandleDragging;
            chestUi.OnItemAction += HandleItemAction;
        }
        else
        {
            Debug.LogWarning("chestUi is not assigned!");
        }
    }

    private void CleanupListeners()
    {
        if (chestUi != null)
        {
            chestUi.OnSwap -= HandleItemSwap;
            chestUi.OnDrag -= HandleDragging;
            chestUi.OnItemAction -= HandleItemAction;
        }

        if (chestData != null)
        {
            chestData.OnInventoryChange -= UpdateChestUI;
        }
    }

    public void PrepareChestData(List<InventoryItem> ItemToInitialize, string chestName)
    {
        Debug.Log("Preparing chest data");
        ChestName = chestName;
        chestData.InstantiateInventory();

      
        if (StateManger.Instance.GetChestState(ChestName) != null) {
            ItemToInitialize = StateManger.Instance.GetChestState(ChestName).ToList();
        }
     

        // Prevent double subscription
        chestData.OnInventoryChange -= UpdateChestUI;
        chestData.OnInventoryChange += UpdateChestUI;

        foreach (var item in ItemToInitialize)
        {
            if (item.IsEmpty)
                continue;

            Debug.Log($"Adding item to chest: {item}");
            chestData.AddItem(item);
        }
        updateChestState();
    }

    private void updateChestState()
    {
        InventoryItem[] ItemState = new InventoryItem[chestData.GetChestState().Count];
        foreach (var item in chestData.GetChestState())
        {
            ItemState[item.Key] = item.Value;
        }

        StateManger.Instance.UpdateChestState(ChestName, ItemState);
    }

    private void HandleDragging(int index)
    {
        InventoryItem item = chestData.GetSpecificItem(index);
        if (item.IsEmpty)
            return;

        chestUi.SetMouse(item.item.Icon, item.quantity);
    }

    private void HandleItemSwap(InventoryUiSlot slot)
    {
        Debug.Log($"Handling item swap: {slot.name}");

        if (DragContext.SourceType == DragSourceType.Inventory && slot.OwnerPage is ChestUiPage)
        {
            if (InventoryController.Instance != null)
            {
                InventoryController.Instance.inventoryData.MoveItemToChest(chestData, slot.SlotIndex, DragContext.SourceIndex);
            }
            else
            {
                Debug.LogWarning("InventoryController.Instance is null!");
            }
        }
        else if (DragContext.SourceType == DragSourceType.Chest && slot.OwnerPage is ChestUiPage)
        {
            chestData.SwapitemPlace(DragContext.SourceIndex, slot.SlotIndex);
        }
        else if (DragContext.SourceType == DragSourceType.AccesorieSlot && slot.OwnerPage is ChestUiPage)
        {
            // Remove accessory from its original accessory slot
            ItemData itemFromAccessorySlot = AccesorieSlotManger.Instance.GetItemAndRemove(DragContext.SourceIndex);
            if (!(itemFromAccessorySlot is AccesoriesItemBase accessoryToDrop))
                return;

            // Get item currently in chest slot
            InventoryItem currentChestItem = chestData.GetSpecificItem(slot.SlotIndex);

            // If there's already an accessory in the chest slot, swap it back into the accessory slot
            if (!currentChestItem.IsEmpty && currentChestItem.item is AccesoriesItemBase)
            {
                // Remove from chest
                chestData.RemoveItem(slot.SlotIndex, 1);

                // Place the old chest item into the accessory slot
                AccesorieSlotManger.Instance.Slots[DragContext.SourceIndex].SetAccesorie(currentChestItem.item as AccesoriesItemBase);

                // Place dragged accessory into chest
                InventoryItem newItem = new InventoryItem
                {
                    item = accessoryToDrop,
                    quantity = 1
                };
                chestData.AddItemToSpecificPos(newItem.item,1,null, slot.SlotIndex);
            }
            else
            {
                // Chest slot is empty or not an accessory — just insert
                InventoryItem newItem = new InventoryItem
                {
                    item = accessoryToDrop,
                    quantity = 1
                };
                chestData.AddItemToSpecificPos(newItem.item,1,null, slot.SlotIndex);
            }

            // Optional: SoundManager.PlaySound(SoundType.SwapItem_Inventory);
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
        Debug.Log("Showing chest");

        if (chestUi != null)
            chestUi.ShowChest();

        ChestUiIsActive = true;

        if (InventoryController.Instance != null && !InventoryController.Instance.InventoryUiActive)
            InventoryController.Instance.InventoryInput();

        UpdateChestUI();
    }

    public void HideChest()
    {
        Debug.Log("Hiding chest");

        if (chestUi != null)
            chestUi.HideChest();

        if (InventoryController.Instance != null && InventoryController.Instance.InventoryUiActive)
            InventoryController.Instance.InventoryInput();

        ChestUiIsActive = false;
    }

    public void UpdateChestUI()
    {
        Debug.Log("Updating Chest UI");

        if (chestUi == null || chestData == null)
        {
            Debug.LogWarning("chestUi or chestData is null!");
            return;
        }

        chestUi.ResetChest();

        foreach (var item in chestData.GetChestState())
        {
            var stats = InventoryController.Instance != null
                ? InventoryController.Instance.GetStatsForTooltip(item.Value)
                : null;

            chestUi.UpdateChestData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description, stats);
        }

        updateChestState();
    }

    private void UpdateChestUI(Dictionary<int, InventoryItem> dictionary)
    {
        if (chestUi == null)
        {
            Debug.LogWarning("chestUi is null in dictionary UpdateChestUI!");
            return;
        }

        chestUi.ResetChest();

        foreach (var item in dictionary)
        {
            var stats = InventoryController.Instance != null
                ? InventoryController.Instance.GetStatsForTooltip(item.Value)
                : null;

            chestUi.UpdateChestData(item.Key, item.Value.item.Icon, item.Value.quantity, item.Value.item.Name, item.Value.item.Description, stats);
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
