using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostLooking : StoneGhostState
{
    public StoneGhostLooking(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    private int LookTimes;

    public override void Enter()
    {
        base.Enter();
        LookTimes = 0;
        controller.onSearch += CheckIfPlayerIsNearby;
        controller.OnPlayAudio += SwitchToBuried;

    }


    public override void Exit()
    {
        base.Exit();
        controller.onSearch -= CheckIfPlayerIsNearby;
        controller.OnPlayAudio -= SwitchToBuried;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SwitchToBuried()
    {
        if (LookTimes >= 12)
            stateMachine.SwitchState(controller.Hide);
    }


    public void CheckIfPlayerIsNearby()
    {
        
        Collider2D hit = Physics2D.OverlapCircle(controller.transform.position, controller.LookingRadius, controller.PlayerMask);
        LookTimes++;
        if (hit != null)
            stateMachine.SwitchState(controller.Rising);
    }
}
