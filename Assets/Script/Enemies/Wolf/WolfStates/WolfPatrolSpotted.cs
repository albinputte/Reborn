using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPatrolSpotted : WolfState
{
    public WolfPatrolSpotted(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
