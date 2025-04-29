using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstances
{
    private int WeaponIndex;

    public WeaponItemData Weapon;

    private OrbsItemData Orbdata;

    private int inventoryPos;

    public WeaponInstances(WeaponItemData weapondata, OrbsItemData OrbData, int WeaponIndex) {
        this.Weapon = weapondata;
        this.Orbdata = OrbData;
        this.WeaponIndex = WeaponIndex;
    }

    public void UpdateInventoryPos(int inventoryPos) => this.inventoryPos = inventoryPos;

    public void UpdateOrb(OrbsItemData orbdata) => this.Orbdata = orbdata;

    public WeaponItemData GetWeapon()
    {
        return this.Weapon;
    }
    public OrbsItemData GetOrb() => this.Orbdata;


}
