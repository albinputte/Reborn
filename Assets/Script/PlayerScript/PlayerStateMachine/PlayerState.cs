using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected string animationName;
    protected PlayerController controller;
    protected int facingDirection = 1;

    public PlayerState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController ) {
        stateMachine = StateMachine;
        playerData = data;
        animationName = animName;
        controller = playerController;
    
    }

    public virtual void Enter()
    {
      
        //Player.animator.play(animationName);  add later when i add player controller script
    }

    public virtual void Exit()
    {

    }
    public virtual void LogicUpdate() 
    {
    
    }

    public virtual void PhysicsUpdate() 
    { 
    
    }

    public void MovementXY()
    {
        controller.rb.velocity = new Vector2(controller.Input.normInputX * playerData.MoveSpeed, controller.Input.normInputY * playerData.MoveSpeed);
    }

    public void CheckFlip()
    {
      
        if(controller.Input.normInputX == 1)
        {
            controller.Parrent.transform.localScale = new Vector3(1, 1, 0);
            facingDirection = 1;
        }
        else if (controller.Input.normInputX == -1)
        {
            controller.Parrent.transform.localScale = new Vector3(-1, 1, 0);
            facingDirection = -1;
        }

    }



}
