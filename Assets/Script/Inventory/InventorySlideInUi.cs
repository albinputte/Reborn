using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlideInUi : MonoBehaviour
{
    public Image WeaponIcon;
    public Image Orbicon;

    public void SetSlideIcons(Sprite WeaponIcon, Sprite  Orbicon)
    {
        this.WeaponIcon.color = new Color(1f, 1f, 1f, 1f);
        this.WeaponIcon.sprite = WeaponIcon;
        if(Orbicon != null) { 
        this.Orbicon.color = new Color(1f, 1f, 1f, 1f);
        this.Orbicon.sprite = Orbicon;
        }
    }
    public void ResetSlideIcons()
    {
        this.WeaponIcon.color = new Color(1f, 1f, 1f, 0f);
        this.WeaponIcon.sprite = null;
        this.Orbicon.color = new Color(1f, 1f, 1f, 0f); 
        this.Orbicon.sprite = null;
    }

    public void RemoveOrbIcon()
    {
        this.Orbicon.sprite = null;
    }


}
