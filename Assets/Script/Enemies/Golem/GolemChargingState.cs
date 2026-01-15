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
        controller.StartPillarCharging();
        time = controller.ChargeTimePhase + Time.time;
    }

    public override void LogicUpdate()
    {
        Debug.Log("Golem " + HasQueuedAbility());
        if (Time.time >= time && HasQueuedAbility())
        {
            stateMachine.SwitchState(
                new GolemExecuteAbilityState(stateMachine, controller)
            );
        }
    }
}