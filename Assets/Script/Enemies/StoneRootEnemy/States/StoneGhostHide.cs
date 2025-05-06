using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoneGhostHide : StoneGhostState
{
    public float MaxTime;
    public float Timer;
    public bool TimerIsActive;
    public StoneGhostHide(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MaxTime = UnityEngine.Random.Range(3, 6);
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
      Collider2D hit =  Physics2D.OverlapCircle(controller.transform.position, controller.StartRangeRadius, controller.PlayerMask);
        if (hit != null)
            stateMachine.SwitchState(controller.Looking);


        TimerIsActive = true;
        Timer = 0;
    }
}
