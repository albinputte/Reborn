using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    public void Hit(int Damage,Vector2 Knockback);
    public void ApplyDamageOverTime(int amountPerTick, float interval, int ticks);




}
