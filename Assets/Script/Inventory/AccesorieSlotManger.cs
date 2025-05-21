using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AccesorieSlotManger : MonoBehaviour
{
    public AccesoriesSlot[] Slots;


    public void SetSlot(AccesoriesItemBase Item)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].IsEmpty)
            {
                Slots[i].SetAccesorie(Item);
                return;
            }
        }
    }


}


