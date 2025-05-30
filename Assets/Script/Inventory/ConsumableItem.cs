using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemBase", menuName = "Items/ConsumableItem", order = 4)]
public class ConsumableItem : ItemData, IitemAction, IConsumable, IDestroyableItem
{
    
    [SerializeReference, SubclassSelector]
    private BaseForBuff buffBase;
    [SerializeField] public int healAmount;

    public bool PerformAction(GameObject Player, WeaponInstances inst)
    {
        Health health = Player.GetComponentInChildren<Health>();
        if (health != null) {
            health.heal(healAmount, false);
            if (buffBase != null)
                buffBase.ApplyBuff();
            return true;
        }
        return false;
    }

  
}
