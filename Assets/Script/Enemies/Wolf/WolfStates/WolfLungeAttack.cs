using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfLungeAttack : WolfState
{
    public WolfLungeAttack(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    bool animDone;

    public override void Enter()
    {
        base.Enter();

        animDone = false;
        controller.OnAnimationDone += Finished;
        controller.OnAnimationActionTrigger += lunge;


    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationDone -= Finished;
        controller.OnAnimationActionTrigger -= lunge;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.chase);
        }
    }
    public void lunge()
    {
        //controller.StartLunge();
    }

    public void Finished()
    {
        animDone = true;
    }
}
