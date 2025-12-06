using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBiteAttack : WolfState
{
    public WolfBiteAttack(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
