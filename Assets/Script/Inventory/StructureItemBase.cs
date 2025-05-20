using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureBase", menuName = "Items/Structure", order = 4)]
public class StructureItemBase : ItemData, IitemAction, IStructure
{

    public GameObject PlacablePrefab;
    private TileManger manger;
    private InventoryController inventoryController;

    public bool PerformAction(GameObject gameObject, WeaponInstances inst)
    {
        return false;
    }

    public bool PerformBuild(int index)
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
        manger = FindAnyObjectByType<TileManger>();
        if (manger != null)
        {
            manger.ActivateBuildMode(PlacablePrefab, this, index);
            if (inventoryController.InventoryUiActive)
                inventoryController.InventoryInput();
            return true;
        }

        return false;
    }
    public bool CancelBuild(int index)
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
        manger = FindAnyObjectByType<TileManger>();
        if (manger != null)
        {
            manger.disableBuildMode(index);
            return true;
        }

        return false;
    }
}
