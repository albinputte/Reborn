using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemChargingState : GolemBaseState
{
    public GolemChargingState(
        EnemyStateMachine<GolemBossController> sm,
        GolemBossController controller
    ) : base(sm, controller, "Charge")
    {
    }
    public float time;

    public override void Enter()
    {
        base.Enter();
        time = controller.ChargeTimePhase + Time.time;
    }

    public override void LogicUpdate()
    {
        if (Time.time >= time && HasQueuedAbility())
        {
            stateMachine.SwitchState(
                new GolemExecuteAbilityState(stateMachine, controller)
            );
        }
    }
}