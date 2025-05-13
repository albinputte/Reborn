using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingManager : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemData> craftingRecipes;
    private int currentCraftingMangerIndex;
    [SerializeField] public InventorySO inventory;
    [SerializeField] private List<CraftingRecipe> availableRecipes;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private GameObject Button;
    [SerializeField] private SpriteRenderer spriteRen;

    [SerializeField] private InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }
    public SoundType CraftSound;

    //delegate add here
    public void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        availableRecipes = craftingRecipes
             .Where(item => item.IsCraftable)
             .SelectMany(item => item.craftingRecipe)
             .ToList();
      
    
    }
   



    public void CraftItem(CraftingRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            SoundManager.PlaySound(CraftSound);
            foreach (var ingredient in recipe.ingredients)
            {
                
                RemoveItemsFromInventory(ingredient.item, ingredient.quantity);
            }
            inventory.AddItem(recipe.resultItem, recipe.resultQuantity, null);
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
        while (inventory.FindItemInInventory(item) != -1 && quantity > 0)
        {
            int indexPos = inventory.FindItemInInventory(item);
            InventoryItem items = inventory.GetSpecificItem(indexPos);

            if (items.quantity <= quantity)
            {
                inventory.RemoveItem(indexPos, items.quantity);
                quantity -= items.quantity;
            }
            else
            {
                inventory.RemoveItem(indexPos, quantity);
                quantity = 0;
            }
        }

    }

    public List<CraftingRecipe> GetAvailableRecipes()
    {
        return availableRecipes;
    }

  

    public void Interact()
    {
        Debug.Log("I interact");
        if (!craftingUI.UiIsActive)
        {
            Debug.Log("I Open");
            currentCraftingMangerIndex = craftingUI.checkFirstEmptySlotInCraftingManger();
            craftingUI.craftingManager[craftingUI.checkFirstEmptySlotInCraftingManger()] = this;
            craftingUI.ShowCraftinUi();
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, false);
        }  
        else
        {
            Debug.Log("I Close");
            craftingUI.HideCraftingUi();
            craftingUI.ClearRecipeInformation();
            craftingUI.craftingManager[currentCraftingMangerIndex] = null;
            craftingUI.UpdateUi(this, currentCraftingMangerIndex, true);
        }
 
    }
    public void NearPlayer()
    {
        spriteRen.material = NewMaterial;
        Button.gameObject.SetActive(true);
    }

    public void LeavingPlayer()
    {
        spriteRen.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }
}