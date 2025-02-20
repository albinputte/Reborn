using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : PlayerState
{
    public ActionState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        IsAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
        controller.Input.ActionPefromed = false;
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //MovementXY();
        if (IsAbilityDone)
        {
            stateMachine.SwitchState(controller.idle);
           
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
