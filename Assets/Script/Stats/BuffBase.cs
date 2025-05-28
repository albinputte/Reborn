using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBase 
{
    public abstract void ApplyBuff();
    public abstract void RemoveBuff();

}
[Serializable]
public class AddetiveBuff : BuffBase
{
    public float bonusMultiplier = 0.2f;
    public StatsType statType;

    public override void ApplyBuff()
    {
        Debug.Log("i Add " + StatSystem.instance.GetStat(statType));
        StatSystem.instance.AddStats(statType, bonusMultiplier);
    }

    public override void RemoveBuff()
    {
        Debug.Log("i Remove " + StatSystem.instance.GetStat(statType));
        StatSystem.instance.RemoveStats(statType, bonusMultiplier);
     
    }
}


