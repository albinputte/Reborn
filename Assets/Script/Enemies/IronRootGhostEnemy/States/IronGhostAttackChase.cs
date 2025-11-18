using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGhostAttackChase : IronGhostState
{
    public IronGhostAttackChase(EnemyStateMachine<IronGhostController> stateMachine, IronGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.OnHitPlayer += () => { stateMachine.SwitchState(controller.Hit); };
        controller.col.enabled = true;

    }

    public override void Exit()
    {
        base.Exit();
        controller.OnHitPlayer -= () => { stateMachine.SwitchState(controller.Hit); };

        controller.col.enabled = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Move(controller.Speed * 2f, controller.transform, controller.Target);
    }
}
