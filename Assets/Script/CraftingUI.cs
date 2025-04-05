using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour

{
    [SerializeField] public CraftingManager craftingManager;
    [SerializeField] private List<CraftingRecipe> availableRecipes;
    private int AmountOfRecipes;
    [SerializeField] private GameObject ParrentToSpawnRecipeunder;
    [SerializeField] private GameObject RecipePrefab;
    [SerializeField] private List<RecipeSlotUi> recipeSlots;
    [SerializeField] private Image[] IngrediantSlot;
    [SerializeField] private Image ResultSlot;
    [SerializeField] private CraftButtonUi craftButtonUi;

    public void AttemptCrafting(CraftingRecipe recipe)
    {
       
            craftingManager.CraftItem(recipe);
    
    }



    public void UpdateUi(CraftingManager crafting)
    {
        craftingManager = crafting;
        availableRecipes.Clear();
        availableRecipes = craftingManager.GetAvailableRecipes();
        AmountOfRecipes = availableRecipes.Count;

        foreach (Transform child in ParrentToSpawnRecipeunder.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < AmountOfRecipes; i++)
        {
            GameObject recipe = Instantiate(RecipePrefab, ParrentToSpawnRecipeunder.transform);
            recipe.GetComponent<RecipeSlotUi>().SetRecipe(availableRecipes[i]);
            recipe.GetComponent<RecipeSlotUi>().SetIndex(i);
            recipeSlots.Add(recipe.GetComponent<RecipeSlotUi>());
            recipeSlots[i].OnItemClicked += SetUpRecipeInfiormation;
        }
     


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
        craftButtonUi.SetRecipe(recipe.recipe);
        craftButtonUi.OnItemClicked += CraftButtonPressed;

    }

    public void CraftButtonPressed(CraftButtonUi button)
    {
        AttemptCrafting(button.recipe);
    }



    public void ClearRecipeInformation()
    {
        for (int i = 0; i < IngrediantSlot.Length; i++)
        {
            IngrediantSlot[i].gameObject.SetActive(false);
        }
        ResultSlot.gameObject.SetActive(false);
        craftButtonUi.gameObject.SetActive(false);
    }



}

