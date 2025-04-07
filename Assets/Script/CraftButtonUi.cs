using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftButtonUi : MonoBehaviour
{
    public event Action<CraftButtonUi> OnItemClicked;

    public CraftingRecipe recipe;
    public int CurrentCraftingMangerIndex;
    public void SetRecipe(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        this.recipe = recipe;
        this.CurrentCraftingMangerIndex = CurrentCraftingMangerIndex;
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
