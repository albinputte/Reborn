using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoflIdle : WolfState
{
    public WoflIdle(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    float IdleTime;

    public override void Enter()
    {
        base.Enter();
        if (controller.chosenAction == WolfController.ActionType.Patrol) {
            IdleTime = Time.time + Random.Range(controller.IdleTime[0], controller.IdleTime[1]);
            controller.LOS.enabled = true;
        }
    }

    public override void LogicUpdate()
    {
       
        base.LogicUpdate();
        if (controller.chosenAction == WolfController.ActionType.Patrol && Time.time >= IdleTime)
        {
            stateMachine.SwitchState(controller.patrol);
        }

    }
}
