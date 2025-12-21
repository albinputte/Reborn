using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GolemExecuteHeal : GolemBaseState
{
    private float exitTime;
    private GolemAbilityType nextAbility;

    public GolemExecuteHeal(
        EnemyStateMachine<GolemBossController> stateMachine,
        GolemBossController controller,
        string animName
    ) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 1️⃣ Prevent instant exit
        exitTime = Time.time + Mathf.Max(0.1f, controller.TimeBetweenPhases);

        // 2️⃣ Execute Creature ability ONLY
        CreaturePillar pillar = GetCreaturePillar();
        pillar?.ExecuteAbility();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time <= exitTime)
            return;

        // 3️⃣ Decide next ability when leaving
        nextAbility = controller.GetNextAbility();

        switch (nextAbility)
        {
            case GolemAbilityType.Meteor:
                stateMachine.SwitchState(
                    new GolemExecuteCrystalRain(stateMachine, controller, "")
                );
                break;

            case GolemAbilityType.Mage:
                stateMachine.SwitchState(
                    new GolemExecuteMage(stateMachine, controller, "")
                );
                break;

            case GolemAbilityType.Creature:
                stateMachine.SwitchState(
                    new GolemExecuteHeal(stateMachine, controller, "")
                );
                break;

            default:
                stateMachine.SwitchState(
                    new GolemIdleState(stateMachine, controller)
                );
                break;
        }
    }

    private CreaturePillar GetCreaturePillar()
    {
        foreach (var pillar in controller.pillars)
        {
            if (pillar is CreaturePillar creaturePillar)
                return creaturePillar;
        }

        Debug.LogWarning("No CreaturePillar found!");
        return null;
    }
}

