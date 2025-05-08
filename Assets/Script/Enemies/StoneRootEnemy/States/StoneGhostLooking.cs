using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostLooking : StoneGhostState
{
    public StoneGhostLooking(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
  

    public override void Enter()
    {
        base.Enter();
        controller.onSearch += CheckIfPlayerIsNearby;
        controller.OnPlayAudio += () => { SoundManager.PlaySound(SoundType.StoneGhost_Looking); };

    }


    public override void Exit()
    {
        base.Exit();
        controller.onSearch -= CheckIfPlayerIsNearby;
        controller.OnPlayAudio -= () => { SoundManager.PlaySound(SoundType.StoneGhost_Looking); };
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }



    public void CheckIfPlayerIsNearby()
    {
        
        Collider2D hit = Physics2D.OverlapCircle(controller.transform.position, controller.LookingRadius, controller.PlayerMask);
        if (hit != null)
            stateMachine.SwitchState(controller.Rising);
    }
}
