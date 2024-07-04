using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InventoryModel inventoryModel;
    void Start()
    {
        inventoryModel = new InventoryModel();
    }

    // Update is called once per frame
    public void AddItemToInventory(ItemData Data, int quantity)
    {

    }
    public void RemoveItemFromInventory(ItemData Data)
    {

    }
}
