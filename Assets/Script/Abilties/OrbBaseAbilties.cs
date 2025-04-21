using UnityEngine;
using System;

public class BaseOrbAbilties : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public BaseOrbAbilties OrbAbilties;
}

[Serializable] // ✅ Required for managed reference serialization
public abstract class BaseAbiltiesOrb
{
    public abstract void ActivateAbilties();
}

[Serializable] // ✅ Required for subclasses
public class OrbTest : BaseAbiltiesOrb
{
    public override void ActivateAbilties()
    {
        Debug.Log("Test ability activated");
    }
}
[Serializable]
public class OrbFireball : BaseAbiltiesOrb
{
    public int damage = 10;

    public override void ActivateAbilties()
    {
        Debug.Log("Fireball activated for " + damage + " damage!");
    }
}
