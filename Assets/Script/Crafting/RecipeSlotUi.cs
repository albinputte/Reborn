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
    [SerializeField] private Image Border;
    [SerializeField] private Sprite[] HighLightBorderSprite;
   
    public UIToolTipTrigger[] toolTipTrigger;
    public UIToolTipTrigger ResultItemToolTip;
    
    public void SetRecipe(CraftingRecipe recipe, int CurrentCraftingMangerIndex)
    {
        recipeIconImage.sprite = recipe.resultItem.Icon;
        ResultItemToolTip.header = recipe.resultItem.Name;
        ResultItemToolTip.content = recipe.resultItem.Description;
        InventoryItem item = new InventoryItem();
        item.item = recipe.resultItem;
        ResultItemToolTip.values = GetStatsForTooltip(item);
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
                toolTipTrigger[i].header = recipe.ingredients[i].item.Name;
                toolTipTrigger[i].content = recipe.ingredients[i].item.Description;
                InventoryItem item = new InventoryItem();
                item.item = recipe.ingredients[i].item;
                toolTipTrigger[i].values = InventoryController.Instance.GetStatsForTooltip(item);

            }
        }
    }
  

    public void SetBorder()
    {
        if (Border.sprite != HighLightBorderSprite[1])
        {
            Border.sprite = HighLightBorderSprite[1];
        }


    }
    public void DeselectBorder()
    {
        if (Border.sprite != HighLightBorderSprite[0])
        {
            Border.sprite = HighLightBorderSprite[0];
        }

    }

    public void NotEnoughResources(int index)
    {
        Color notEnoughIngredientColor;
        Color notEnoughTextColor;

        ColorUtility.TryParseHtmlString("#FFFFFF60", out notEnoughIngredientColor);
        ColorUtility.TryParseHtmlString("#7b2536", out notEnoughTextColor);

        ingrediants[index].color = notEnoughIngredientColor;
        QuanityText[index].color = notEnoughTextColor;
    }

    public void EnoughResource(int index)
    {
        Color enoughColor;
        ColorUtility.TryParseHtmlString("#dae1e5", out enoughColor);

        ingrediants[index].color = enoughColor;
        QuanityText[index].color = enoughColor;
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
