
public class BatIdleState : BaseEnemyState<BatEnemyController>
{
    public BatIdleState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void LogicUpdate()
    {
        if (controller.PlayerInDetectionRange())
            stateMachine.SwitchState(controller.AlertState);
    }
}
