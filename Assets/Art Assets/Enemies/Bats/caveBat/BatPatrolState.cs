
public class BatPatrolState : BatbaseState
{
    public BatPatrolState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void PhysicsUpdate()
    {
        //controller.CirclePlayer();
    }

    public override void LogicUpdate()
    {
        if (!controller.PlayerInDetectionRange())
            stateMachine.SwitchState(controller.IdleState);
        else if (controller.DistanceToPlayer <= controller.fleeRange)
            stateMachine.SwitchState(controller.FleeState);
        else if (controller.PlayerInAttackRange() && controller.CanAttack)
            stateMachine.SwitchState(controller.AttackState);
    }
}
