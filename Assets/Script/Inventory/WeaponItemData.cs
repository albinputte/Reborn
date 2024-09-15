using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
   
    public WeaponAttackSprite[] WeaponAttackSprites;


    

}

