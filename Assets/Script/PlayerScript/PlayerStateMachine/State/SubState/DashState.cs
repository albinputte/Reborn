using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : ActionState
{

    float dashtime;
    float currentTime;
    Vector2 directionToDash;
    
    public DashState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashtime = playerData.DashTime + Time.time;
        
        
    }

    public override void Exit()
    {
        controller.Input.IsDashing = false;
        base.Exit();
        controller.Input.ActionPefromed = false;
        
      

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        currentTime = Time.time;
        if (dashtime < currentTime)
        {
            IsAbilityDone = true;
        }
        else {
            Dash();
        }
    }

   
}
