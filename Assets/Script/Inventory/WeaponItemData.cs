using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "WeaponItemBase", menuName = "WeaponItem", order = 3)]

public class WeaponItemData : ItemData
{
   
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

    

}

public enum WeaponType
{
    none = 0,
    Sword = 1,
    Axe = 2,
    Bow = 3

}

