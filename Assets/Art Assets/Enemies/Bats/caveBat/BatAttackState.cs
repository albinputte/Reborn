
public class BatAttackState : BatbaseState
{
    private bool hasFired;

    public BatAttackState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void Enter()
    {
        base.Enter();
        controller.ResetStateTimer();
        hasFired = false;
    }

    public override void LogicUpdate()
    {
        controller.TickStateTimer();

        if (!hasFired && controller.stateTimer >= 0.2f)
        {
            controller.FireSoundWave();
            hasFired = true;
        }

        if (controller.stateTimer >= 0.6f)
        {
            stateMachine.SwitchState(controller.FleeState);
        }
    }
}

