using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemExecuteLaser : GolemBaseState
{
    public GolemExecuteLaser(EnemyStateMachine<GolemBossController> stateMachine, GolemBossController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    private float exitTime;
    private GolemAbilityType nextAbility;



    public override void Enter()
    {
        base.Enter();

        // 1️⃣ Prevent instant exit
        exitTime = Time.time + Mathf.Max(0.1f, controller.TimeBetweenPhases[3]);

        // 2️⃣ Execute Meteor ability ONLY
        WallPillar pillar = GetWallPillar();
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

            case GolemAbilityType.Wall:
                stateMachine.SwitchState(
                    new GolemExecuteLaser(stateMachine, controller, "")
                    );
                break;

            default:
                stateMachine.SwitchState(
                    new GolemIdleState(stateMachine, controller)
                );
                break;
        }
    }

    private WallPillar GetWallPillar()
    {
        foreach (var pillar in controller.pillars)
        {
            if (pillar is WallPillar meteorPillar)
                return meteorPillar;
        }

        Debug.LogWarning("No MeteorPillar found!");
        return null;
    }
}
