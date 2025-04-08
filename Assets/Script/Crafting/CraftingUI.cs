using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] public CraftingManager[] craftingManager;
    private Dictionary<int, List<CraftingRecipe>> availableRecipes = new();
    private int AmountOfRecipes;

    [SerializeField] private GameObject ParrentToSpawnRecipeunder;
    [SerializeField] private GameObject RecipePrefab;
    [SerializeField, HideInInspector] private List<RecipeSlotUi> recipeSlots;
    [SerializeField] private Image[] IngrediantSlot;
    [SerializeField] private Image ResultSlot;
    [SerializeField] private CraftButtonUi craftButtonUi;

    public void AttemptCrafting(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        craftingManager[CurrentCraftingMangerIndex].CraftItem(recipe);
    }

    public void UpdateUi(CraftingManager crafting, int CurrentCraftingMangerIndex, bool IsActive)
    {
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

                recipeSlots.Add(slotUi);
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
        for (int i = 0; i < IngrediantSlot.Length; i++)
        {
            IngrediantSlot[i].gameObject.SetActive(true);
            IngrediantSlot[i].sprite = recipe.recipe.ingredients[i].item.Icon;
        }

        ResultSlot.sprite = recipe.recipe.resultItem.Icon;
        ResultSlot.gameObject.SetActive(true);

        craftButtonUi.gameObject.SetActive(true);
        craftButtonUi.SetRecipe(recipe.recipe, recipe.CurrentCraftingMangerIndex);
        craftButtonUi.OnItemClicked += CraftButtonPressed;
    }

    public void CraftButtonPressed(CraftButtonUi button)
    {
        AttemptCrafting(button.recipe, button.CurrentCraftingMangerIndex);
    }

    public void ClearRecipeInformation()
    {
        foreach (var slot in IngrediantSlot)
        {
            slot.gameObject.SetActive(false);
        }

        ResultSlot.gameObject.SetActive(false);
        craftButtonUi.gameObject.SetActive(false);
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
        gameObject.SetActive(false);
    }

    public void ShowCraftinUi()
    {
        gameObject.SetActive(true);
    }
}
