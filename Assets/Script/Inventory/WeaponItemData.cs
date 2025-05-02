using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "WeaponItemBase", menuName = "Items/WeaponItem", order = 3)]

public class WeaponItemData : ItemData, IitemAction, IWeapon
{

    [SerializeReference, SubclassSelector]
    public List<BuffBase> buffs = new List<BuffBase>();
    [SerializeField]
    public SoundType[] attackSounds;

    [Serializable]
    public struct WeaponAttackSprite
    {
       public Sprite[] AttackSprite;
    }
    [Header("Weapon Info")]
   
    [Space(12)]
    public int Damage;
    public float KnockbackForce ;
    public WeaponType WeaponType;
    [Space(0)]
    [Header("Direction indexes:  Righ/left = 0, top = 1, down = 2")]
    public WeaponAttackSprite[] WeaponAttackSprites;

    private void Reset()
    {
        itemType = ItemType.Weapon;
        IsStackable = false;
        WeaponAttackSprites = new WeaponAttackSprite[3];
    }

  
    public bool PerformAction(GameObject gameObject, WeaponInstances inst)
    {
        Debug.Log("Performing action with weapon: " + gameObject.name);
        PlayerWeaponAgent agent = gameObject.GetComponentInChildren<PlayerWeaponAgent>();
        if (agent != null)
        {
            Debug.Log("Setting weapon: " + gameObject.name);
            agent.SetWeapon(inst);
            return true;
        }
        Debug.LogWarning("PlayerWeaponAgent component not found in the game object.");
        return false;
    }
}



public enum WeaponType
{
    none = 0,
    Sword = 1,
    Axe = 2,
    Bow = 3

}

