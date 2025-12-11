using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootEnterBurnMode : GrootState
{
    public GrootEnterBurnMode(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public bool animDone;


    public override void Enter()
    {
        base.Enter();
        BurnMode = true;
        animDone = false;
        controller.BurnMode = BurnMode; 
        controller.OnAnimatioDone += Finished;

    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimatioDone -= Finished;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.BurnIdle);
        }
    }

    public void Finished()
    {
        animDone = true;
    }
}
