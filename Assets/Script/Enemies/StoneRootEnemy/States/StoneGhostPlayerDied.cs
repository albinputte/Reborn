using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostPlayerDied : StoneGhostState
{


    public bool animDone;
    public StoneGhostPlayerDied(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

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
            stateMachine.SwitchState(controller.Hide);
        }
    }

    public void Finished()
    {
        animDone = true;
    }
}
