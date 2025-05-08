using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OrbItemBase", menuName = "Items/Orbs", order = 3)]
public class OrbsItemData : ItemData, IitemAction, IDestroyableItem, IOrb
{

    [SerializeReference, SubclassSelector]
    public BaseAbiltiesOrb abiltiesOrb;
    public PlayerWeaponAgent weaponAgent;

    public bool PerformAction(GameObject gameObject, WeaponInstances inst)
    {
       
        PlayerWeaponAgent agent = gameObject.GetComponentInChildren<PlayerWeaponAgent>();
        if (agent != null)
        {
            agent.EquipOrb(this);
            return true;
        }
  
        return false;
    }
}
