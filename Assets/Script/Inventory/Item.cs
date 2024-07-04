using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public ItemData Data;
    public int Quantity;

   
    public Item(ItemData data, int amount)
    {
        this.Data = data;
        this.Quantity = amount;
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        Quantity -= quantity;
    }

}
