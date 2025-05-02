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

        HandleAttackInput();
        HandleMovementInput();
        HandleInteractionInput();

        MovementXY();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
