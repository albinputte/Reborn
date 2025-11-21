using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostNormalChase : StoneGhostState
{
 
    public StoneGhostNormalChase(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public float MaxTime;
    public float Timer;
    public bool TimerIsActive;
    public bool prefersToChase;
    public override void Enter()
    {
        base.Enter();
        prefersToChase = Random.value < 0.6f;
        if (IsDamaged)
        {
            MaxTime = 3f;
        }
        else {
            MaxTime = 1f;
        }
        
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
        if (IsDamaged) { MoveFromPlayer(controller.Speed, controller.transform, controller.Target); }//RaycastMovement(controller.Speed, controller.transform, controller.Target, prefersToChase); }
        else { Move(controller.Speed, controller.transform, controller.Target); }
            if (TimerIsActive)
        {
            Timer += Time.deltaTime;
            if (Timer >= MaxTime)
                stateMachine.SwitchState(controller.EnterAttack);

        }
    }
}
