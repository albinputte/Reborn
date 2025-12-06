using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootIdle : GrootState
{
    public GrootIdle(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        BrainNormalMode();
    }
}
