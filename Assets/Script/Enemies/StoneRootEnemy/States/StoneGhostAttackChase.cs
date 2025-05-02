using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostAttackChase : StoneGhostState
{
    public StoneGhostAttackChase(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Move(controller.Speed *1.5f, controller.transform, controller.Target);
    }
}
