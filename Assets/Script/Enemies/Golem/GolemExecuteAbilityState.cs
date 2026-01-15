using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemExecuteAbilityState : GolemBaseState
{
    public GolemExecuteAbilityState(
        EnemyStateMachine<GolemBossController> sm,
        GolemBossController controller
    ) : base(sm, controller, "Attack")
    {
    }

    public override void Enter()
    {
        base.Enter();

        GolemAbilityType ability = controller.GetNextAbility();
        Debug.Log("uWU " +  ability);
        Debug.Log(ability.ToString());
        switch (ability)
        {
            case GolemAbilityType.Meteor:
                stateMachine.SwitchState(new GolemExecuteCrystalRain(stateMachine, controller, ""));
                break;
            case GolemAbilityType.Mage:
                stateMachine.SwitchState(new GolemExecuteMage(stateMachine, controller, ""));
                break;
            case GolemAbilityType.Creature:
                stateMachine.SwitchState(new GolemExecuteHeal(stateMachine, controller, ""));
                break;
            case GolemAbilityType.Wall:
                stateMachine.SwitchState(
                    new GolemExecuteLaser(stateMachine, controller, "")
                    );
                break;
            case GolemAbilityType.none:
                stateMachine.SwitchState(new GolemIdleState(stateMachine, controller));
                break;
        }


        }
    }
