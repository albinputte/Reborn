using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfPatrol : WolfState
{
    public WolfPatrol(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.chosenAction = WolfController.ActionType.Patrol;
        controller.LOS.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();
        controller.PatrolDone = false;
        controller.IsPatrolling = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (controller.PatrolDone)
        {
            stateMachine.SwitchState(controller.idle);
        }
        else
        {
            controller.Patrol();
        }

       
    }
}
