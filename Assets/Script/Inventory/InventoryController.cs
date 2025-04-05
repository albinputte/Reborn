using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryUiPage inventoryUi;

    [SerializeField]
    private InventorySO inventoryData;
    private bool InventoryUiActive;

    public ItemData testItem;

    public List<InventoryItem> ItemToInitialize  = new List<InventoryItem>();

    private void Start()
    {
        PrepareInventoryUI();
        PrepareInventoryData();
      
    }

    private void PrepareInventoryUI()
    {
        inventoryUi.InstantiateInventory();
        inventoryUi.OnSwap += HandleItemSwap;
        inventoryUi.OnDrag += HandleDragging;
    }

    private void PrepareInventoryData()
    {
        inventoryData.InstantiateInventory();
        inventoryData.OnInventoryChange += UpdateInventoryUI;
        foreach(var item in ItemToInitialize)
        {
            if(item.IsEmpty)
                continue;
            Debug.Log(item.ToString());
            inventoryData.AddItem(item);
        }
        }
    

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> dictionary)
    {
        inventoryUi.ResetInventory();
        foreach(var item in dictionary)
        {
            inventoryUi.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);
        }
    }

    private void HandleDragging(int index)
    {
        
        InventoryItem item = inventoryData.GetSpecificItem(index);
        if (item.IsEmpty)
            return;

        inventoryUi.SetMouse(item.item.Icon,item.quantity);
    }

    private void HandleItemSwap(int index1,int Index2)
    {
        Debug.Log(Index2);
        Debug.Log(index1);
        inventoryData.SwapitemPlace(index1,Index2);
    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!InventoryUiActive)
            { 
                inventoryUi.ShowInventory(); 

                InventoryUiActive = true;
                foreach(var item in inventoryData.GetInventoryState())
                {
                    inventoryUi.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);

                }
            }
            else if (InventoryUiActive)
            {
                inventoryUi.HideInventory();
                InventoryUiActive = false;
            }

        }
     
    }
}
