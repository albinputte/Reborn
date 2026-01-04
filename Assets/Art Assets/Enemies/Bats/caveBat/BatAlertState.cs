
public class BatAlertState : BaseEnemyState<BatEnemyController>
{
    public BatAlertState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void Enter()
    {
        controller.ResetStateTimer();
    }

    public override void LogicUpdate()
    {
        controller.TickStateTimer();
        if (controller.stateTimer >= 0.6f)
            stateMachine.SwitchState(controller.PatrolState);
    }
}
