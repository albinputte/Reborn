using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GolemExecuteMage : GolemBaseState
{
    public GolemExecuteMage(EnemyStateMachine<GolemBossController> stateMachine, GolemBossController controller, string animName) : base(stateMachine, controller, animName)
    {
    }



    private float exitTime;
    private GolemAbilityType nextAbility;

    public override void Enter()
    {
        base.Enter();

        // 1️⃣ Ensure we don't instantly exit the state
        exitTime = Time.time + Mathf.Max(0.1f, controller.TimeBetweenPhases[2]);

        // 2️⃣ Execute Mage ability (do NOT get next ability yet)
        MagePillar pillar = GetMagePillar();
        pillar?.ExecuteAbility();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 3️⃣ Wait until the timer expires
        if (Time.time <= exitTime)
            return;

        // 4️⃣ Decide the NEXT ability only when leaving this state
        nextAbility = controller.GetNextAbility();

        // 5️⃣ Switch state safely
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

            case GolemAbilityType.none:
                stateMachine.SwitchState(
                    new GolemIdleState(stateMachine, controller)
                );
                break;
        }
    }

    // 6️⃣ Correct pillar selection
    private MagePillar GetMagePillar()
    {
        foreach (var pillar in controller.pillars)
        {
            if (pillar is MagePillar magePillar)
                return magePillar;

        }
        return null;
    }
}
