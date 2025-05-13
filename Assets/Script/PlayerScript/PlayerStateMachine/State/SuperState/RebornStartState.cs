using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebornStartState : PlayerState
{
    public RebornStartState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.OnAnimationDone += () => { stateMachine.SwitchState(controller.idle); };
    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationDone -= () => { stateMachine.SwitchState(controller.idle); };
    }
}
