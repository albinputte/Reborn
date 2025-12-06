using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState : BaseEnemyState<WolfController>
{
    public WolfState(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    private enum ActionType { None, Chase, Attack, Patrol, ReturnToTerritory }

    private ActionType chosenAction = ActionType.None;
    private bool actionInProgress = false;

    public override void Enter()
    {
        base.Enter();
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
