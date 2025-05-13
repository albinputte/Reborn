using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestinteractionManger : MonoBehaviour, IInteractable
{
    public ChestController controller;
    public List<InventoryItem> ItemToInitialize = new List<InventoryItem>();
    [SerializeField] public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }

    public void Interact()
    {
        if (controller.ChestUiIsActive)
        {
            controller.HideChest();
            UpdateItems();
        }
        else
        {
            controller.PrepareChestData(ItemToInitialize);
            controller.ShowChest();

        }
    }

    public void UpdateItems()
    {
        ItemToInitialize = controller.chestData.Inventory;
    }

   
}
