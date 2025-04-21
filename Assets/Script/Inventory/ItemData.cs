using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemBase", menuName = "Items/BaseItem", order = 2)]
public class ItemData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public ItemType itemType;
    public bool IsStackable;
    public bool IsCraftable;
    public List<CraftingRecipe> craftingRecipe;

    public int ID => GetInstanceID();


}
public enum ItemType
{
    Resources,
    Consumables,
    Food,
    Potion,
    Weapon,
    Armour,
    accessory,
    Misc,
    Orbs

}
[Serializable]
public struct CraftingRecipe
{
    public List<RecipeIngredient> ingredients;
    public ItemData resultItem;
    public int resultQuantity;
}

[Serializable]
public struct RecipeIngredient
{
    public ItemData item;
    public int quantity;
}
