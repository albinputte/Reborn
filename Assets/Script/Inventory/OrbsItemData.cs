using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OrbItemBase", menuName = "Items/Orbs", order = 3)]
public class OrbsItemData : ItemData
{

    [SerializeReference, SubclassSelector]
    public BaseAbiltiesOrb abiltiesOrb; 

}
