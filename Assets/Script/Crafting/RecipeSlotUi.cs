using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Image[] ingrediants;
    [SerializeField] private TextMeshProUGUI[] QuanityText;
    public void SetRecipe(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        recipeIconImage.sprite = recipe.resultItem.Icon;
        this.recipe = recipe;
        this.CurrentCraftingMangerIndex = CurrentCraftingMangerIndex;
        SetIngrediants(recipe);
    }
    public void SetIndex(int index)
    {
        recipeIndex = index;
    }

    public void SetIngrediants(CraftingRecipe recipe)
    {
        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            if (i <= ingrediants.Length)
            {
                ingrediants[i].gameObject.SetActive(true);
                QuanityText[i].gameObject.SetActive(true) ;
                ingrediants[i].sprite = recipe.ingredients[i].item.Icon;
                QuanityText[i].text = recipe.ingredients[i].quantity.ToString();
            }
        }
    }

    public void NotEnoughResources(int index)
    {
        ingrediants[index].color = new Color(1f, 1, 1f, 0.1f);
        QuanityText[index].color = new Color(1f, 0, 0f, 1);
    }

    public void EnoughResource(int index)
    {
        ingrediants[index].color = new Color(1f, 1, 1f, 1f);
        QuanityText[index].color = new Color(1f, 1, 1f, 1);
    }


    public Image[] GetIngrediantsSlots()
    {
        return ingrediants;
    }

    public void OnPointerClick(BaseEventData data)
    {

        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnItemClicked?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }
}
