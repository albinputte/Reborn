using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftButtonUi : MonoBehaviour
{
    public event Action<CraftButtonUi> OnItemClicked;

    public CraftingRecipe recipe;
    public void SetRecipe(CraftingRecipe recipe)
    {
        this.recipe = recipe;
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
