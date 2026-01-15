using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatbaseState : BaseEnemyState<BatEnemyController>
{
    public BatbaseState(EnemyStateMachine<BatEnemyController> stateMachine, BatEnemyController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.animator.Play(animName);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
