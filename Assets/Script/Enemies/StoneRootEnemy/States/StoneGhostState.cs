using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostState : BaseEnemyState<StoneGhostController>
{
    public StoneGhostState(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        controller.anim.Play(animName);
        controller.onTakeDamage += Hit;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Respawn.instance.isRespawning)
        {
            if(stateMachine.CurrentState != controller.Hide || stateMachine.CurrentState != controller.PlayerDied)
            {
                stateMachine.SwitchState(controller.PlayerDied);
            }
               
        }

       
    }

    protected void Move(float speed, Transform transform, Transform player)
    {
        float move = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, move);
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public override void Exit()
    {
        base.Exit();
        controller.onTakeDamage -= Hit;
    }

    public void Hit()
    {
        stateMachine.SwitchState(controller.Hit);
    }
}
