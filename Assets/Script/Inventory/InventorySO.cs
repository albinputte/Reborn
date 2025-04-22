using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    public List<InventoryItem> Inventory;

    [SerializeField] private int size;
    [SerializeField] public int MaxItemStack;

    public Action<Dictionary<int, InventoryItem>> OnInventoryChange;

    public void InstantiateInventory()
    {
        Inventory = new List<InventoryItem>();

        for (int i = 0; i < size - 1; i++)
        {
            Inventory.Add(InventoryItem.CreateEmptyItem());
        }

    }
    public int AddItem(ItemData item, int quantity, WeaponInstances instances)
    {

        if (!item.IsStackable)
        {
       
            for (int i = 0; i < Inventory.Count; i++)
            {
                Debug.Log(inventoryIsFull());
                while (quantity > 0 && inventoryIsFull() == false)
                {
                
                    FindNearestEmptyItem(item, 1, instances);
                    quantity--;
                }
                OnInventoryStateChange();
                return quantity;

            }

        }

        quantity = StackItem(item, quantity);
   
        OnInventoryStateChange();
        return quantity;


    }

    public void addWeaponinstance(int index, WeaponInstances instances)
    {
        InventoryItem item = Inventory[index];
        item.weaponInstances = instances;
        Inventory[index] = item;
    }

    public void UpdateOrb(int index, ItemData Orb)
    {
        InventoryItem item = Inventory[index];
        item.weaponInstances.UpdateOrb(Orb);
        Inventory[index] = item;
    }
    public WeaponInstances CreateWeaponIntances(ItemData item, ItemData orb, int index)
    {
        if (item is WeaponItemData weaponData)
            return new WeaponInstances(weaponData, orb, index);

        Debug.LogWarning("Tried to create weapon instance with non-weapon item");
        return null;
    }

    public InventoryItem GetSpecificItem(int index)
    {
       
            return Inventory[index];
      
        
        
            
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if (Inventory.Count >= itemIndex)
        {
            int remaningAmount = Inventory[itemIndex].quantity - amount;
            if (remaningAmount <= 0)
            {
                Inventory[itemIndex] = InventoryItem.CreateEmptyItem();

            }
            else
            {
                Inventory[itemIndex].ChangeQuantity(remaningAmount);
            }
            OnInventoryStateChange();
        }


    }

    public int StackItem(ItemData item, int quantity)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].IsEmpty)
                continue;
            if (item.ID == Inventory[i].item.ID)
            {
                int PossibleAmountToTake = MaxItemStack - Inventory[i].quantity;

                if (quantity > PossibleAmountToTake)
                {

                    Inventory[i] = Inventory[i].ChangeQuantity(MaxItemStack);
                    quantity -= PossibleAmountToTake;
                }
                else
                {
                    Inventory[i].ChangeQuantity(Inventory[i].quantity + quantity);
                    OnInventoryStateChange();
                    return 0;
                }

            }

        }
     
        while (quantity > 0 && inventoryIsFull() == false)
        {
    
            int newQuantity = Mathf.Clamp(quantity, 0, MaxItemStack);
     
            quantity -= newQuantity;
          
            FindNearestEmptyItem(item, newQuantity, null);

        }
        return quantity;
    }

    public void SwapitemPlace(int item1, int item2)
    {
       
        InventoryItem temp = Inventory[item1];
        Inventory[item1] = Inventory[item2];
        Inventory[item2] = temp;
        OnInventoryStateChange();
    }

    public bool inventoryIsFull() => Inventory.Where(item => item.IsEmpty).Any() == false;

    public int FindNearestEmptyItem(ItemData items, int quantity, WeaponInstances instances)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = items,
            quantity = quantity,
            weaponInstances = instances
        };

        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].IsEmpty)
            {    
                Inventory[i] = newItem;
                if (newItem.item is WeaponItemData weaponData && newItem.weaponInstances == null)
                {
                    addWeaponinstance(i, CreateWeaponIntances(weaponData, null, i));
                }
                return quantity;
            }

        }

        return 0;
    }

    public void OnInventoryStateChange() { OnInventoryChange?.Invoke(GetInventoryState()); }


    public Dictionary<int, InventoryItem> GetInventoryState()
    {
        Dictionary<int, InventoryItem> Returnvalue = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].IsEmpty)
                continue;
            Returnvalue[i] = Inventory[i];

        }
        return Returnvalue;
    }

    internal void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity, item.weaponInstances);
    }
}
[Serializable]
public struct InventoryItem
{
    public ItemData item;
    public int quantity;
    public bool IsEmpty => item == null;
    public WeaponInstances weaponInstances;

    public InventoryItem ChangeQuantity(int Newquantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = Newquantity
        };
    }

    public static InventoryItem CreateEmptyItem() => new InventoryItem { item = null, quantity = 0 }; 


}
