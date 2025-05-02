using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostLooking : StoneGhostState
{
    public StoneGhostLooking(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    public float MaxTime;
    public float Timer;
    public bool TimerIsActive;

    public override void Enter()
    {
        base.Enter();
        MaxTime = 0.5f;
        Timer = 0;
        TimerIsActive = false;
        CheckIfPlayerIsNearby();

    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (TimerIsActive)
        {
            Timer += Time.deltaTime;
            if (Timer >= MaxTime)
                CheckIfPlayerIsNearby();
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }



    public void CheckIfPlayerIsNearby()
    {
        TimerIsActive = false;
        Collider2D hit = Physics2D.OverlapCircle(controller.transform.position, controller.StartRangeRadius, controller.PlayerMask);
        if (hit != null)
            stateMachine.SwitchState(controller.Rising);


        TimerIsActive = true;
        Timer = 0;
    }
}
