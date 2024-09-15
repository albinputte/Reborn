using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseMovementStates
{
    public MoveState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
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
        CheckFlip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
