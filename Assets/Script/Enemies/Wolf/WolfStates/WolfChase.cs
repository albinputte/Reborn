using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChase : WolfState
{
    public WolfChase(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
