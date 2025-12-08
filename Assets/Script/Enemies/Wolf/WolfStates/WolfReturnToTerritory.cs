using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfReturnToTerritory : WolfState
{
    public WolfReturnToTerritory(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.MoveToHomeNode();
    }

    public override void Exit()
    {
        base.Exit();
        controller.chosenAction = WolfController.ActionType.Patrol;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (controller.currentPath.Count <= 1)
        {
            stateMachine.SwitchState(controller.idle);
        }
        else 
        {
            controller.MoveToHomeNode();    
        }
    }
}
