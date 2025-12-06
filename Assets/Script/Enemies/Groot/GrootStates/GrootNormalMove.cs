using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootNormalMove : GrootState
{
    public GrootNormalMove(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
