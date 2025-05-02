using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostState : BaseEnemyState<StoneGhostController>
{
    public StoneGhostState(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

       
    }

    protected void Move(float speed, Transform transform, Transform player)
    {
        float move = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, move);
    }
}
