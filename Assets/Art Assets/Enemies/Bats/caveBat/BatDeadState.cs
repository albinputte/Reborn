
public class BatDeadState : BaseEnemyState<BatEnemyController>
{
    public BatDeadState(EnemyStateMachine<BatEnemyController> sm, BatEnemyController c, string a)
        : base(sm, c, a) { }

    public override void Enter()
    {
        controller.enabled = false;
        var col = controller.GetComponent<UnityEngine.Collider2D>();
        if (col != null) col.enabled = false;
    }
}
