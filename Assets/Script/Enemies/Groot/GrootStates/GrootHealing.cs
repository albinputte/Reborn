using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootHealing : GrootState
{
    public GrootHealing(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public bool animDone;
    public int TimesHealed;


    public override void Enter()
    {
        base.Enter();
        animDone = false;
        TimesHealed = 0;
        //add knockback
        controller.OnAnimatioDone += heal;

    } 

    public override void Exit()
    {
        IdleTime = controller.GetTime() + Random.Range(controller.TimeInBetweenAttack[0], controller.TimeInBetweenAttack[1]);
        base.Exit();
        IsPeforminAction = false;
        controller.OnAnimatioDone -= heal;
  
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (TimesHealed == controller.TimesToheal && animDone || controller.Health.GetCurrentHealth() == controller.MaxHealth)
        {
            stateMachine.SwitchState(controller.Idle);
        }
    }

    public void heal()
    {
       
        controller.OnAnimatioDone -= heal;
        controller.Health.heal((int)controller.Healamount, false);
        TimesHealed++;
        if (TimesHealed == controller.TimesToheal)
        {
            animDone = true;
        }
        controller.OnAnimatioDone += heal;

    }
}
