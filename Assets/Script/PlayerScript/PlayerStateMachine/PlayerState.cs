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
    protected bool IsAbilityDone;
   


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
        if(!controller.Input.isSprinting) 
        {    
        controller.rb.velocity = new Vector2(controller.Input.normInputX * playerData.moveSpeed, controller.Input.normInputY * playerData.moveSpeed);
        }
        else
        {
            controller.rb.velocity = new Vector2(controller.Input.normInputX * playerData.runSpeed, controller.Input.normInputY * playerData.runSpeed);
        }
    }

  

    public void CheckFlip()
    {

        if (controller.Input.normInputX == 1)
        {
            controller.Parrent.transform.localScale = new Vector3(1, 1, 0);
            PlayerController.FacingDirection.x = 1;
           
        }
        else if (controller.Input.normInputX == -1)
        {
            controller.Parrent.transform.localScale = new Vector3(-1, 1, 0);
            PlayerController.FacingDirection.x = -1;
          
        }

        if (controller.Input.normInputX != 0 && controller.Input.normInputY == 0 )
        {
            PlayerController.FacingDirection.y = 0;
        }

        else if (controller.Input.normInputY == 1)
        {
            PlayerController.FacingDirection.y = 1;
        }
        else if (controller.Input.normInputY == -1)
        {
            PlayerController.FacingDirection.y = 2;
        }





    }

 



}
