using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpeedBuff : BuffBase
{
    public float Modfier;
    public bool isTimeBased;
    public float BuffTime;

    public override void ApplyBuff()
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveBuff()
    {
        throw new NotImplementedException();
    }

}
