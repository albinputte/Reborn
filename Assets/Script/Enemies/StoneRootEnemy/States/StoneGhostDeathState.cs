using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostDeathState : StoneGhostState
{
    public StoneGhostDeathState(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        controller.OnAnimationDone += () => { GameObject.Destroy(controller.gameObject); };
    }

    public override void Exit()
    {
        base.Exit();
    }
}
