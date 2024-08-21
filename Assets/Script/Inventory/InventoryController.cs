using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryUiPage inventoryUi;
    private bool InventoryUiActive;

    public void Awake()
    {
        //inventoryUi.InstantiateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!InventoryUiActive)
            { 
                inventoryUi.ShowInventory(); 
                InventoryUiActive = true;
            }
            else if (InventoryUiActive)
            {
                inventoryUi.HideInventory();
                InventoryUiActive = false;
            }

        }
     
    }
}
