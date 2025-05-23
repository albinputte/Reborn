using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButtonUi : MonoBehaviour
{
    public event Action<CraftButtonUi> OnItemClicked;
    public event Action RefreshCrafting;
    [SerializeField] private Sprite[] ButtonSprites;
    [SerializeField] private Image Buttonimage; 
    public CraftingRecipe recipe;
    public int CurrentCraftingMangerIndex;
    public bool HasBeenClicked;
    public void Start()
    {
        Buttonimage = GetComponent<Image>();
    }
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
                StartCoroutine(ActivateDelay());
                OnItemClicked?.Invoke(this);
                RefreshCrafting?.Invoke();
               
            }
           
        }
    }

    public IEnumerator ActivateDelay()
    {
        Buttonimage.sprite = ButtonSprites[1];
        HasBeenClicked = false;
        yield return new WaitForSeconds(0.4f);
        HasBeenClicked = true;
        Buttonimage.sprite = ButtonSprites[0];
    }


}
