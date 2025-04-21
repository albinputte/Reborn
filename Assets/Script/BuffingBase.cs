using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffingBase : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public BaseForBuff speedBuff;

}

public abstract class BaseForBuff
{
    public abstract void ApplyBuff();
}

[Serializable]
public class speedBuff : BaseForBuff
{
    public override void ApplyBuff()
    {
        throw new NotImplementedException();
    }

}
[Serializable]
public class HealthBuff : BaseForBuff
{
    public override void ApplyBuff()
    {
        throw new NotImplementedException();
    }
}



