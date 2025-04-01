using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLootTable : MonoBehaviour
{
    public DropTable dropTable;

    void Start()
    {
        Table table = dropTable.LootingTable[0];
        List<ItemData> droppedItems = dropTable.RollLoot(table);
        foreach (ItemData item in droppedItems)
        {
            Debug.Log(item.Name);
        }
    }


}
