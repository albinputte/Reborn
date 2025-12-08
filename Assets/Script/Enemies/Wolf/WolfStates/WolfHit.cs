using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHit : WolfState
{
    public WolfHit(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    public bool animDone;


    public override void Enter()
    {
        base.Enter();

        animDone = false;
        controller.OnAnimationDone += Finished;
  


    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationDone -= Finished;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.chase);
        }
    }

    public void Finished()
    {
        animDone = true;
    }
}
