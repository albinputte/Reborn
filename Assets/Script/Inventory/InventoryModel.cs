using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel 
{
    public Dictionary<int, Item> inventory;
  
    public InventoryModel()
    {
        inventory = new Dictionary<int, Item>();
    }

    public void addItem(Item item, int index)                                                                                 
    {
        inventory.Add(index, item);
    }

    public void removeItem(int index)
    {
        inventory.Remove(index);
    }

    public Dictionary<int,Item> GetItems()
    {
        return inventory;
    }




}
