using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotUi : MonoBehaviour
{
    public event Action<RecipeSlotUi> OnItemClicked;

    public int recipeIndex;
    public Image recipeIconImage;
    public CraftingRecipe recipe;
    public int CurrentCraftingMangerIndex;
    public void SetRecipe(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        recipeIconImage.sprite = recipe.resultItem.Icon;
        this.recipe = recipe;
        this.CurrentCraftingMangerIndex = CurrentCraftingMangerIndex;
    }
    public void SetIndex(int index)
    {
        recipeIndex = index;
    }

    public void OnPointerClick(BaseEventData data)
    {

        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            // Right click detected
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }
}
