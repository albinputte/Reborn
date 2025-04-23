using JetBrains.Annotations;
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

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField] 
    private GameObject itemPrefab; //For Dropping item

    [SerializeField]
    private bool InventoryUiActive;

    [SerializeField]
    private CraftingUI craftingUI; //temporary

   
    public ItemData testItem;

    public GameObject Character;

    public List<InventoryItem> ItemToInitialize  = new List<InventoryItem>();

    private void Start()
    {
     
        PrepareInventoryUI();
        PrepareInventoryData();

    }

    private void PrepareInventoryUI()
    {
        inventoryUi.ShowInventory(); 
        inventoryUi.InstantiateInventory();
        inventoryUi.OnSwap += HandleItemSwap;
        inventoryUi.OnDrag += HandleDragging;
        inventoryUi.OnItemAction += HandleItemSelection;
        inventoryUi.OnDropItem += HandleDropIitem;
        inventoryUi.HideInventory();
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

    public WeaponInstances CreateWeaponIntances(ItemData item, ItemData orb, int index)
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

    private void HandleItemSelection(int index1)
    {
        InventoryItem item = inventoryData.GetSpecificItem(index1);
        if (item.IsEmpty)
            return;

        IDestroyableItem iDestroyableItem = item.item as IDestroyableItem;
        if (iDestroyableItem != null)
        {
            inventoryData.RemoveItem(index1, 1);
        }

        IitemAction iitemAction = item.item as IitemAction;
        if (iitemAction != null)
        {
            Debug.Log(item.weaponInstances.Weapon);
            iitemAction.PerformAction(Character, item.weaponInstances);
        }
     
        
    }

    public void HandleDropIitem(int index)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - Character.transform.position).normalized;

        InventoryItem item = inventoryData.GetSpecificItem(index);
        inventoryData.RemoveItem(index, item.quantity);
        GameObject obj = Instantiate(itemPrefab, Character.transform.position + direction * 0.5f, Quaternion.identity); // spawn a little offset from character
        obj.GetComponent<WorldItem>().SetItem(item.item, item.quantity);
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 10;
        rb.AddForce(direction * 10, ForceMode2D.Impulse);

    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!InventoryUiActive)
            { 
                inventoryUi.ShowInventory(); 
                craftingUI.ShowCraftinUi();

                InventoryUiActive = true;
                foreach(var item in inventoryData.GetInventoryState())
                {
                    inventoryUi.UpdateData(item.Key, item.Value.item.Icon, item.Value.quantity);
                   
                }
            }
            else if (InventoryUiActive)
            {
                inventoryUi.HideInventory();
                craftingUI.HideCraftingUi();
                InventoryUiActive = false;
            }

        }
     
    }

}
