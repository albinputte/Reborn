using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> craftingRecipes;
    private int currentCraftingMangerIndex;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private List<CraftingRecipe> availableRecipes;
    [SerializeField] private CraftingUI craftingUI;
    
    //delegate add here
    public void Start()
    {
        availableRecipes = craftingRecipes
             .Where(item => item.IsCraftable)
             .SelectMany(item => item.craftingRecipe)
             .ToList();
    
    }
   



    public void CraftItem(CraftingRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            foreach (var ingredient in recipe.ingredients)
            {
                RemoveItemsFromInventory(ingredient.item, ingredient.quantity);
            }
            inventory.AddItem(recipe.resultItem, recipe.resultQuantity);
            Debug.Log("Crafted " + recipe.resultItem.name);
            //delegate add here
        }
        else
        {
            //delegate add here
            Debug.Log("failed");
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        Dictionary<ItemData, int> inventoryItems = inventory.GetInventoryState()
            .Where(item => !item.Value.IsEmpty)
            .GroupBy(item => item.Value.item)
            .ToDictionary(group => group.Key, group => group.Sum(i => i.Value.quantity));

        return recipe.ingredients.All(ingredient =>
            inventoryItems.ContainsKey(ingredient.item) && inventoryItems[ingredient.item] >= ingredient.quantity);
    }

    private void RemoveItemsFromInventory(ItemData item, int quantity)
    {
        for (int i = 0; i < inventory.Inventory.Count && quantity > 0; i++)
        {
            if (inventory.Inventory[i].item == item)
            {
                int removeAmount = Mathf.Min(quantity, inventory.Inventory[i].quantity);
                inventory.RemoveItem(i, removeAmount);
                quantity -= removeAmount;
            }
        }
    }

    public List<CraftingRecipe> GetAvailableRecipes()
    {
        return availableRecipes;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentCraftingMangerIndex = craftingUI.checkFirstEmptySlotInCraftingManger();
            craftingUI.craftingManager[craftingUI.checkFirstEmptySlotInCraftingManger()] = this;
            craftingUI.gameObject.SetActive(true);
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, false);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
    
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);
        }
    }
}