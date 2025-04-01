using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTableBase", menuName = "DropTable", order = 2)]
public class DropTable : ScriptableObject
{
    public Table GurantedLoot;
    public List<Table> LootingTable;

    // Simulates a loot roll and returns the dropped items
    public List<ItemData> RollLoot(Table table)
    {
        List<ItemData> droppedItems = new List<ItemData>();
        droppedItems = GurantedLoot.lootTable.ConvertAll(x => x.item);

        foreach (var loot in GurantedLoot.lootTable)
        {
            int quantity = Random.Range(loot.minDrop, loot.maxDrop + 1);
            for (int i = 0; i < quantity; i++)
            {
                droppedItems.Add(loot.item);
            }
        }


        foreach (var loot in table.lootTable)
        {
            float roll = Random.Range(0f, 100f); // Random chance between 0 and 100
            if (roll <= loot.dropChance)
            {
                int quantity = Random.Range(loot.minDrop, loot.maxDrop + 1);
                for (int i = 0; i < quantity; i++)
                {
                    droppedItems.Add(loot.item);
                }
            }
        }

        return droppedItems;
    }
}

[System.Serializable]
public struct Table
{
    public string Name;
    public int ChanceToRoll;
    public List<LootTable> lootTable;

}

[System.Serializable]
public struct LootTable
{
    public ItemData item;
    public float dropChance;
    public int minDrop;
    public int maxDrop;
}
