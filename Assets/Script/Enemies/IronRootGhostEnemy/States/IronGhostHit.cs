using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGhostHit : IronGhostState
{
    public IronGhostHit(EnemyStateMachine<IronGhostController> stateMachine, IronGhostController controller, string animName) : base(stateMachine, controller, animName)
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
            {
                if (WasAttacked == 0)
                {
                    stateMachine.SwitchState(controller.NormalChase);
                }
                else
                {
                    stateMachine.SwitchState(controller.EnterAttack);
                    WasAttacked = 0;
                }

            }


        }


    }
}
