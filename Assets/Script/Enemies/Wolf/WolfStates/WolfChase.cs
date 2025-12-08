using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChase : WolfState
{
    public WolfChase(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (controller.GetDistanceBetweenEnemyAndObject(controller.player) < 1.5F)
        {
            stateMachine.SwitchState(controller.attack);
        }
        else if (controller.GetDistanceBetweenEnemyAndObject(controller.HomeNode.transform) > 40)
        {
            stateMachine.SwitchState(controller.ReturnToTerritory);
        }
        else {
            controller.MoveTowardPlayer();
        }
    }
}
