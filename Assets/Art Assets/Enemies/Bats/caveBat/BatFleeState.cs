
public class BatFleeState : BatbaseState
{
    public BatFleeState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void Enter()
    {
        base.Enter();
        controller.ResetStateTimer();
    }

   

    public override void LogicUpdate()
    {
        controller.FleeFromPlayerPathfinding(controller.fleeSpeed);
        controller.TickStateTimer();

        if (controller.stateTimer >= controller.fleeDuration)
        {
            stateMachine.SwitchState(controller.PatrolState);
        }
    }
}

