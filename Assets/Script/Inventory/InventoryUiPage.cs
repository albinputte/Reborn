using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUiPage : MonoBehaviour
{
    [SerializeField]
    private InventoryUiSlot[] itemSlot;

    List<InventoryUiSlot> ListOfUIslots = new List<InventoryUiSlot>();


    private void Start()
    {
        InstantiateInventory();
    }

    public void InstantiateInventory()
    {
        itemSlot = FindObjectsOfType<InventoryUiSlot>();
        ListOfUIslots = itemSlot.OrderBy(slot => ExtractNumberFromName(slot.name)).ToList();


    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
    }
    public void HideInventory() 
    {
        gameObject.SetActive(false); 
    }

    private int ExtractNumberFromName(string name)
    {
        
        int number = 0;
        string numStr = new string(name.Where(char.IsDigit).ToArray());
        int.TryParse(numStr, out number);
        return number;
    }


}
