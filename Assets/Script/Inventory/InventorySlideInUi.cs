using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlideInUi : MonoBehaviour
{
    public Image WeaponIcon;
    public Image Orbicon;
    public OrbSlotUi[] Slot;
    public static InventorySlideInUi Instance;
    [SerializeField] private InventoryUiPage InvUiPage;
    private Animator anim;
    private bool IsOpen;
    public void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        InvUiPage = GetComponentInParent<InventoryUiPage>();
    }
    private void Start()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].OnItemBeginDrag += OnItemDrag;
            Slot[i].OnItemEndDrag += OnEndDrag;
        }
    }
    public void OpenSlider()
    {
        if (!IsOpen)
        {
            anim.SetTrigger("Open");
            IsOpen = true;
        }
          
    }
    public void CloseSlider()
    {
        if (IsOpen)
        {
            anim.SetTrigger("Close");
            IsOpen = false;
        }
       
    }

    public void OpenAndCloseSliderButton()
    {
        if (!IsOpen)
        {
            anim.SetTrigger("Open");
            IsOpen = true;
        }
        else
        {
            anim.SetTrigger("Close");
            IsOpen = false;
        }
    }

    public void SetSlideIcons(WeaponInstances Instances)
    {
        this.WeaponIcon.color = new Color(1f, 1f, 1f, 1f);
        this.WeaponIcon.sprite = Instances.Weapon.Icon;
        for (int i = 0; i < Slot.Length; i++)
        {
            if (Slot[i].IsEmpty)
            {
                if (Instances.GetOrb() != null)
                    Slot[i].SetOrb(Instances.GetOrb());
                return;
            }
        }

        
      
    }
    public void ResetSlideIcons()
    {
        this.WeaponIcon.color = new Color(1f, 1f, 1f, 0f);
        this.WeaponIcon.sprite = null;
        for (int i = 0; i < Slot.Length; i++)
        {

            if (Slot[i].GetItemFromSlot() != null)
                Slot[i].RemoveOrb(false, false);
        }
    }

    public ItemData GetItemAndRemove(int slot)
    {
        ItemData item = Slot[slot].GetItemFromSlot();
        Slot[slot].RemoveOrb(false, true);
        return item;
    }

    public void OnItemDrag(OrbSlotUi slot)
    {
        int index = Array.IndexOf(Slot, slot);

        DragContext.SourceType = DragSourceType.OrbSlot;
        DragContext.SourceIndex = index;
        InvUiPage.SetMouse(Slot[index].GetItemFromSlot().Icon, 1);

    }

    public void OnEndDrag(OrbSlotUi slot)
    {
        DragContext.SourceIndex = -1;
        InvUiPage.ResetMouse();
    }




}
