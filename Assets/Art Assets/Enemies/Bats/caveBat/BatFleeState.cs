
public class BatFleeState : BaseEnemyState<BatEnemyController>
{
    public BatFleeState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void PhysicsUpdate()
    {
        UnityEngine.Vector2 dir = (controller.transform.position - controller.Player.position).normalized;
        controller.transform.position += (UnityEngine.Vector3)dir * controller.fleeSpeed * UnityEngine.Time.deltaTime;
    }

    public override void LogicUpdate()
    {
        if (controller.DistanceToPlayer > controller.attackRange)
            stateMachine.SwitchState(controller.PatrolState);
    }
}
