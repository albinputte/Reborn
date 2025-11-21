using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostEnterAttack : StoneGhostState
{

    public bool animDone;
    public StoneGhostEnterAttack(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animDone = false;
        controller.OnAnimationDone += Finished;
        // GeneratePath(controller.GhostTransform, controller.Target);
    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationDone -= Finished;
        SoundManager.PlaySound(SoundType.StoneGhost_StartAttack);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.AttackChase);
        }
    }

    public void Finished()
    {
        animDone = true;
    }
}
