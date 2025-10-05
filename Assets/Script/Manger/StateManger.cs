using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManger : MonoBehaviour
{
    public static StateManger Instance;
    public static Dictionary<string, InventoryItem[]> ChestState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ChestState = new Dictionary<string, InventoryItem[]>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateChestState(string ChestName, InventoryItem[] data)
    {
        Debug.Log(data.Length);

        if (ChestState.ContainsKey(ChestName))
        {
            ChestState[ChestName] = data;
            Debug.Log("Chest Updated");
        }
        else
        {
            ChestState.Add(ChestName, data);
            Debug.Log("New Chest");
        }

     
    }

    public InventoryItem[] GetChestState(string ChestName)
    {
        if (ChestState.TryGetValue(ChestName, out var data))
        {
            return data;
        }

        return null; // Or return new ItemData[0];
    }

  
}
