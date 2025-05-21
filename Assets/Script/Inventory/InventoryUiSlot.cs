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
    private string ItemName;
    private string ItemDescription;
    public bool IsSelected;
    public event Action<InventoryUiSlot> OnItemClicked,
           OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
           OnRightMouseBtnClick, OnItemSelect;
    public bool IsHotbar;

    private bool empty = true;
    public UIToolTipTrigger uitipTrigger;
    public int SlotIndex { get; private set; }
    public object OwnerPage { get; private set; }

    public void Init(object owner, int index)
    {
        OwnerPage = owner;
        SlotIndex = index;
    }


    public void Awake()
    {
        ResetItemData();
        BorderSprite[1] = BorderImage.sprite;
        DeselectBorder();
        uitipTrigger = GetComponent<UIToolTipTrigger>();

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
        OnItemSelect?.Invoke(this);
    }
    public void SetData(Sprite ItemImage, int Quantity, string name, string Desccription)
    {
        itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = ItemImage;
        quantityText.text = Quantity.ToString();
        this.ItemDescription = Desccription;
        this.ItemName = name;

        if (uitipTrigger != null && !IsHotbar)
        {
            uitipTrigger.header = name;
            uitipTrigger.content = Desccription;
        }
     

        empty = false;
    }






    public void OnBeginDrag()
    {
       
        if (empty)
        {
            return;
        }
        uitipTrigger.Dragging();
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        UIToolTipTrigger.IsDragging = false;
        OnItemEndDrag?.Invoke(this);    
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
            OnRightMouseBtnClick?.Invoke(this);
        }
    }

}
