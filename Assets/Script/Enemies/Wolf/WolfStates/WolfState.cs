using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState : BaseEnemyState<WolfController>
{
    public WolfState(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }


   

    public override void Enter()
    {
        base.Enter();
        controller.animator.Play(animName);
        controller.OnTakeDamage += Hit;
    
    }

    public override void Exit()
    {
        base.Exit();
        controller.OnTakeDamage -= Hit;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
       
 
        if (controller.chosenAction == WolfController.ActionType.Patrol && controller.iSePlayer) {
            controller.stateMachine.SwitchState(controller.spotted);
        }
    }
    public void Hit()
    {
    
        stateMachine.SwitchState(controller.hit);

    }
    public void Death()
    {
        stateMachine.SwitchState(controller.death);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
