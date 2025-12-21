using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIdleState : GolemBaseState
{
    public GolemIdleState(
        EnemyStateMachine<GolemBossController> sm,
        GolemBossController controller
    ) : base(sm, controller, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        controller.StartPillarCharging();

        stateMachine.SwitchState(
            new GolemChargingState(stateMachine, controller)
        );
    }
}
