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
        ResetFlip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(controller.Input.normInputX == 0 && controller.Input.normInputY == 0)
        {
            stateMachine.SwitchState(controller.idle);
        }
        else if (controller.Input.isSprinting)
        {
            stateMachine.SwitchState(controller.run);
        }
        else 
        { stateMachine.SwitchState(controller.move); }

        if (controller.Input.IsAttacking)
            controller.stateMachine.SwitchState(controller.baseAttack);

      
        if (controller.Input.isInteracting) 
            if(GetNearestInteractable(2f, controller.InteractionLayer) != null)
                controller.stateMachine.SwitchState(controller.interactBush);
        else
            controller.Input.isInteracting = false; controller.Input.ActionPefromed = false;
        //HandleFacingDirection();
        //CalculateFacingDir();
        MovementXY();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
