using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestinteractionManger : MonoBehaviour, IInteractable
{
    public ChestController controller;
    public List<InventoryItem> ItemToInitialize = new List<InventoryItem>();
    [SerializeField] public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private GameObject Button;
    public SpriteRenderer ChestRenderer;


    public void Start()
    {
        controller = GetComponent<ChestController>();
        ChestRenderer = GetComponent<SpriteRenderer>();
    }
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
    public void NearPlayer()
    {
        ChestRenderer.material = NewMaterial;
        Button.gameObject.SetActive(true);
    }

    public void LeavingPlayer()
    {
        ChestRenderer.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }

}
