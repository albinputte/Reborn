using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootEnterNormalAttack : GrootState
{
    public GrootEnterNormalAttack(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
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
        base.Exit();
        controller.OnAnimatioDone -= Finished;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.NormalAttack);
        }
    }

    public void Finished()
    {
        animDone = true;
    }

}
