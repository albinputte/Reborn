using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootNormalAttack : GrootState
{
    public GrootNormalAttack(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    public bool animDone;
    public override void Enter()
    {
        base.Enter();
        IsPeforminAction = true;
        animDone = false;
        controller.OnAnimatioDone += Finished;
    

    }

    public override void Exit()
    {
        IdleTime = controller.GetTime() + Random.Range(controller.TimeInBetweenAttack[0], controller.TimeInBetweenAttack[1]);
        base.Exit();
        controller.OnAnimatioDone -= Finished;
       

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.Idle);
        }
    }

    public void Finished()
    {
        animDone = true;
    }
}
