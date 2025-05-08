using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net.NetworkInformation;
using UnityEngine.EventSystems;

public class InventoryUiSlot : MonoBehaviour
{


    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite[] BorderSprite = new Sprite[2];
    [SerializeField] private Image BorderImage; // the border that will appear when you click on an item slot :)
    public bool IsSelected;
    public event Action<InventoryUiSlot> OnItemClicked,
           OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
           OnRightMouseBtnClick;

    private bool empty = true;


    public void Awake()
    {
        ResetItemData();
        BorderSprite[1] = BorderImage.sprite;
        DeselectBorder();
        

    }

    public void ResetItemData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void DeselectBorder()
    {
        BorderImage.sprite = BorderSprite[1];
        IsSelected = false;

    }
    public void SelectBorder()
    {
        BorderImage.sprite = BorderSprite[0];
    }
    public void SetData(Sprite ItemImage, int Quantity)
    {
        itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = ItemImage;
        quantityText.text = Quantity.ToString();
        empty = false;
    }




    public void OnBeginDrag()
    {
       
        if (empty)
        {
            return;
        }
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);    
    }

    public void OnPointerClick(BaseEventData data)
    {
       
        PointerEventData pointerData = data as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

}
