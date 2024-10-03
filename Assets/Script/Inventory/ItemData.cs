using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemBase", menuName = "Item", order = 2)]
public class ItemData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public ItemType itemType;
    public bool IsStackable;


    public int ID => GetInstanceID();







}
public enum ItemType
{
    Resources,
    Consumables,
    Food,
    Potion,
    Weapon,
    Armour,
    accessory,
    Misc
}