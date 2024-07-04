using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiViewer : MonoBehaviour
{
    [Serializable]
    public struct InventorySlots
    {
        public int slotNumber;
        public Image itemSlotSprite;
        
    }
    public InventorySlots[] slots; 
    public SlotIndex[] slotIndex;
    void Start()
    {
        slots = new InventorySlots[32];
        slotIndex = FindObjectsByType<SlotIndex>(FindObjectsSortMode.None);

        foreach (SlotIndex slot in slotIndex)
        {
            slots[slot.slotNumber].slotNumber = slot.slotNumber;
            slots[slot.slotNumber].itemSlotSprite = slot.gameObject.GetComponentInChildren<Image>();
            Debug.Log(slot.gameObject.GetComponentInChildren<Image>().gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
