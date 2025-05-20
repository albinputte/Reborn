using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesoriesItemBase : ItemData, IitemAction, IDestroyableItem, IAccesories
{

    [SerializeReference, SubclassSelector]
    public BuffBase BuffBase;
    public PlayerWeaponAgent weaponAgent;

    public bool PerformAction(GameObject gameObject, WeaponInstances inst)
    {

        return false;
    }
}
