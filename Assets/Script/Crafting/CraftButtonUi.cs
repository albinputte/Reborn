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
    public bool HasBeenClicked;
    public void SetRecipe(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        this.recipe = recipe;
        this.CurrentCraftingMangerIndex = CurrentCraftingMangerIndex;
        HasBeenClicked = true;
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
            if (HasBeenClicked)
            {
                OnItemClicked?.Invoke(this);
                StartCoroutine(ActivateDelay());
            }
           
        }
    }

    public IEnumerator ActivateDelay()
    {
        HasBeenClicked = false;
        yield return new WaitForSeconds(0.3f);
        HasBeenClicked = true;
    }


}
