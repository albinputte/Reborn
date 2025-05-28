using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "AccesoriesBase", menuName = "Items/Accesories", order = 5)]
public class AccesoriesItemBase : ItemData, IDestroyableItem, IAccesories
{

    [SerializeReference, SubclassSelector]
    public BuffBase BuffBase;

    public void EquipAccesorie()
    {
        BuffBase.ApplyBuff();
    }

    public void RemoveAccesorie()
    {
        Debug.Log("remove buff");
        BuffBase.RemoveBuff();
    }
}
