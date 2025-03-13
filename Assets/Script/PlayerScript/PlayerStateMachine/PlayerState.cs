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

    

    }

    public void CalculateFacingdir()
    {
        Vector2 direction = controller.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (degree > 45 && degree < 135)
        {
            PlayerController.FacingDirection.y = 2; //down
        }
        else if (degree > 135 || degree < -135)
        {
            PlayerController.FacingDirection.y = 0;
            PlayerController.FacingDirection.z = 1;
            if (controller.Input.normInputX == 0)
            {
                controller.Parrent.transform.localScale = new Vector3(1, 1, 0);
                PlayerController.FacingDirection.x = 1;
            }
        }
        else if (degree < -45 && degree > -135)
        {
            PlayerController.FacingDirection.y = 1;// up
        }
        else if (degree < 45 && degree > -45)
        {
            PlayerController.FacingDirection.y = 0;
            PlayerController.FacingDirection.z = -1;
            if (controller.Input.normInputX == 0)
            {
                controller.Parrent.transform.localScale = new Vector3(-1, 1, 0);
                PlayerController.FacingDirection.x = -1;
            }
        }
    }





}
