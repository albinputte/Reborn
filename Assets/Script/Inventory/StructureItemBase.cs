using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureBase", menuName = "Items/Structure", order = 4)]
public class StructureItemBase : ItemData, IitemAction, IDestroyableItem
{

    public GameObject PlacablePrefab;
    private TileManger manger;
    private InventoryController inventoryController;

    public bool PerformAction(GameObject gameObject, WeaponInstances inst)
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
        manger = FindAnyObjectByType<TileManger>();
        if (manger != null)
        {
            manger.ActivateBuildMode(PlacablePrefab);
            if(inventoryController.InventoryUiActive)
                inventoryController.InventoryInput();
            return true;
        }

        return false;
    }
}
