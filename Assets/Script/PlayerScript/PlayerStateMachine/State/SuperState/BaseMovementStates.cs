using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovementStates : PlayerState
{
    public BaseMovementStates(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(controller.rb.velocity.x == 0 && controller.rb.velocity.y == 0)
        {
            stateMachine.SwitchState(controller.idle);
        }
        else if (controller.Input.isSprinting)
        {
            stateMachine.SwitchState(controller.run);
        }
        else 
        { stateMachine.SwitchState(controller.move); }

        MovementXY();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
