using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]private DropTable dropTable;
    [SerializeField]private GameObject itemPrefab;
    [SerializeField]private List<ItemData> DropList;

    public void ItemDrop(Transform transform)
    {
        Table table = dropTable.LootingTable[0];
        DropList = dropTable.RollLoot(table);
        foreach (ItemData item in DropList)
        {
            GameObject obj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<WorldItem>().SetItem(item,1);

        }
     
    }


}
