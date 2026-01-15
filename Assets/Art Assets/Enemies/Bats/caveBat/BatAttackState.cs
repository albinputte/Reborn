
public class BatAttackState : BatbaseState
{
    public BatAttackState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void Enter()
    {
        controller.FireSoundWave();
        stateMachine.SwitchState(controller.PatrolState);
    }
}
