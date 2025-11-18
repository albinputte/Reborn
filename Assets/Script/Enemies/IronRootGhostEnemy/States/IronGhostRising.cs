using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGhostRising : IronGhostState
{
    public bool RisingDone;
    public IronGhostRising(EnemyStateMachine<IronGhostController> stateMachine, IronGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        RisingDone = false;
        controller.OnAnimationDone += Finished;
        controller.OnPlayAudio += () => { SoundManager.PlaySound(SoundType.StoneGhost_Rising); };
    }

    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationDone -= Finished;
        controller.OnPlayAudio -= () => { SoundManager.PlaySound(SoundType.StoneGhost_Rising); };
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (RisingDone)
        {
            stateMachine.SwitchState(controller.NormalChase);
        }
    }

    public void Finished()
    {
        RisingDone = true;
    }
}
