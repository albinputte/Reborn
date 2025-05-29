using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] public CraftingManager[] craftingManager;
    private Dictionary<int, List<CraftingRecipe>> availableRecipes = new();
    private int AmountOfRecipes;
    public bool UiIsActive;
    [SerializeField] private GameObject Frame;
    [SerializeField] private GameObject ParrentToSpawnRecipeunder;
    [SerializeField] private GameObject RecipePrefab;
    [SerializeField, HideInInspector] private List<RecipeSlotUi> recipeSlots;
    [SerializeField] private Image ResultSlot;
    [SerializeField] private CraftButtonUi craftButtonUi;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [HideInInspector]public bool IsInteractingCrafting;
    [HideInInspector] public bool InteractButtonClicked;
    public static CraftingUI Instance { get; private set; }
    private RecipeSlotUi CurrentSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Start()
    {
        HideAtStart();
      
    }

    public void AttemptCrafting(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        craftingManager[CurrentCraftingMangerIndex].CraftItem(recipe);
    }

    public void UpdateUi(CraftingManager crafting, int CurrentCraftingMangerIndex, bool IsActive)
    {
        layoutGroup.enabled = true;
        if (IsActive)
        {
            // Deactivate/remove this manager
            craftingManager[CurrentCraftingMangerIndex] = null;

            if (availableRecipes.ContainsKey(CurrentCraftingMangerIndex))
            {
                availableRecipes.Remove(CurrentCraftingMangerIndex);
            }
        }
        else
        {
            // Add or update this manager
            craftingManager[CurrentCraftingMangerIndex] = crafting;

            if (crafting == null)
                return;

            var newRecipes = crafting.GetAvailableRecipes();

            // Check for changes
            if (availableRecipes.ContainsKey(CurrentCraftingMangerIndex) &&
                AreRecipesEqual(availableRecipes[CurrentCraftingMangerIndex], newRecipes))
            {
                return; // No change, skip
            }

            availableRecipes[CurrentCraftingMangerIndex] = newRecipes;
        }

        // Update the UI from all active managers
        RefreshCraftingUI();
    }

    private void RefreshCraftingUI()
    {
        // Clear existing UI
        foreach (Transform child in ParrentToSpawnRecipeunder.transform)
        {
            Destroy(child.gameObject);
        }

        recipeSlots.Clear();

        // Create new slots starting from newest (last added manager shown on top)
        List<int> sortedKeys = new List<int>(availableRecipes.Keys);
        sortedKeys.Sort(); // optional: reverse or custom sort

        foreach (int managerIndex in sortedKeys)
        {
            var recipes = availableRecipes[managerIndex];

            for (int i = 0; i < recipes.Count; i++)
            {
                GameObject recipe = Instantiate(RecipePrefab, ParrentToSpawnRecipeunder.transform);
                var slotUi = recipe.GetComponent<RecipeSlotUi>();
                slotUi.SetRecipe(recipes[i], managerIndex);
                slotUi.SetIndex(recipeSlots.Count);
                slotUi.OnItemClicked += SetUpRecipeInfiormation;
                SetIfEnoughResources(slotUi);

                recipeSlots.Add(slotUi);
            }
        }
    
    }
    private void RefreshUiWithoutResteting()
    {

        {
            foreach (var slotUi in recipeSlots)
            {
                SetIfEnoughResources(slotUi);
            }
        }

    }




    private bool AreRecipesEqual(List<CraftingRecipe> oldList, List<CraftingRecipe> newList)
    {
        if (oldList.Count != newList.Count)
            return false;

        for (int i = 0; i < oldList.Count; i++)
        {
            if (!oldList[i].Equals(newList[i])) // Ensure CraftingRecipe implements Equals
                return false;
        }

        return true;
    }

    public void SetUpRecipeInfiormation(RecipeSlotUi recipe)
    {

        layoutGroup.enabled = false;
        if (CurrentSlot == null)
        {
            CurrentSlot = recipe;
            CurrentSlot.SetBorder();
        }
        else
        {
            CurrentSlot.DeselectBorder();
            CurrentSlot = recipe;
            CurrentSlot.SetBorder();
        }

        SetIfEnoughResources(recipe);
        ResultSlot.sprite = recipe.recipe.resultItem.Icon;
        ResultSlot.gameObject.SetActive(true);

        // Update craft button logic
        craftButtonUi.RefreshCrafting -= RefreshUiWithoutResteting;
        craftButtonUi.OnItemClicked -= CraftButtonPressed;
        craftButtonUi.SetRecipe(recipe.recipe, recipe.CurrentCraftingMangerIndex);
        craftButtonUi.RefreshCrafting += RefreshUiWithoutResteting;
        craftButtonUi.OnItemClicked += CraftButtonPressed;
       
    }


    public void SetIfEnoughResources(RecipeSlotUi recipe)
    {
        for (int i = 0; i < recipe.recipe.ingredients.Count; i++)
        {

            if (!IsItemQuantityInInventory(recipe.recipe.ingredients[i].item, recipe.recipe.ingredients[i].quantity))
            {
                recipe.NotEnoughResources(i);
            }
            else
            {
                recipe.EnoughResource(i);
            }


        }

    }

    private bool IsItemQuantityInInventory(ItemData item, int requiredQuantity)
    {
        int totalQuantity = 0;

        // Loop through the inventory and add up quantities of matching items
        for (int i = 0; i < craftingManager[0].inventory.Inventory.Count; i++)
        {
            InventoryItem invItem = craftingManager[0].inventory.GetSpecificItem(i);
            if (invItem.IsEmpty != true && invItem.item == item)
            {
                totalQuantity += invItem.quantity;
            }
        }
     
        // Return true if total quantity meets or exceeds required quantity
        return totalQuantity >= requiredQuantity;
    }

    public void CraftButtonPressed(CraftButtonUi button)
    {
        AttemptCrafting(button.recipe, button.CurrentCraftingMangerIndex);
    }

    public void ClearRecipeInformation()
    {

        ResultSlot.gameObject.SetActive(false);
        //craftButtonUi.gameObject.SetActive(false);
    }

    public int checkFirstEmptySlotInCraftingManger()
    {
        for (int i = 0; i < craftingManager.Length; i++)
        {
            if (craftingManager[i] == null)
                return i;
        }

        return -1;
    }

    public void HideCraftingUi()
    {
        Frame.SetActive(false);
        UiIsActive = false;
        
    }
    public void HideAtStart()
    {
        Frame.SetActive(false);
    }

    public void ShowCraftinUi()
    {
        layoutGroup.enabled = true;
        Frame.SetActive(true);
        UiIsActive = true;
    
    }
}
