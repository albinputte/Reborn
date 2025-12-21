using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemBaseState
    : BaseEnemyState<GolemBossController>
{
    protected GolemBaseState(
        EnemyStateMachine<GolemBossController> stateMachine,
        GolemBossController controller,
        string animName
    ) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        // Common logic for ALL golem states
        controller.animator?.Play(animName);
    }

    protected void FacePlayer()
    {
        // Optional helper if you ever rotate the boss
    }

    protected bool HasQueuedAbility()
    {
        return controller.CanExecuteAbility();
    }
}
