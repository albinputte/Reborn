using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootBurnIdle : GrootState
{
    public GrootBurnIdle(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        BrainBurnMode();
    }
}
