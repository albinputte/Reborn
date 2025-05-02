using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostHit : StoneGhostState
{
    public StoneGhostHit(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
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
        TimerIsActive = true;
       

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
                stateMachine.SwitchState(controller.NormalChase);
        }


    }
}
