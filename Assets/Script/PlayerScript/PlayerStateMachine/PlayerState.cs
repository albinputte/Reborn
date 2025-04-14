using System;
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
    protected Vector2 CurrentVelocity;
    protected bool IsAbilityDone;
    protected Directions directions;
    
   


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
       CurrentVelocity = controller.rb.velocity;
    }

    public virtual void PhysicsUpdate() 
    { 
     
    }

    public void MovementXY()
    {
        if(!controller.Input.isSprinting) 
        {
            controller.rb.velocity = new Vector2(controller.Input.moveDir.x * Mathf.Lerp(0, playerData.moveSpeed,1f), controller.Input.moveDir.y * Mathf.Lerp(0, playerData.moveSpeed, 1f));
            CurrentVelocity = new Vector2(controller.Input.moveDir.x * Mathf.Lerp(0, playerData.moveSpeed, 1f), controller.Input.moveDir.y * Mathf.Lerp(0, playerData.moveSpeed, 1f));
        }
        else
        {
            controller.rb.velocity = new Vector2(controller.Input.normInputX * playerData.runSpeed, controller.Input.normInputY * playerData.runSpeed);
        }
    }

    public void SetVelocity(Vector2 vel)
    {
        controller.rb.velocity = new Vector2(vel.x, vel.y);
        CurrentVelocity = new Vector2(vel.x, vel.y);
    }

    public void SetDrag(int dragAmount)
    {
        controller.rb.drag = dragAmount;    
    }

  

    public void CheckFlip()
    {

        if (controller.Input.normInputX == 1)
        {
            controller.Parrent.transform.localScale = new Vector3(1, 1, 0);
        }
        else if (controller.Input.normInputX == -1)
        {
            controller.Parrent.transform.localScale = new Vector3(-1, 1, 0);
        }

    

    }

    public void CalculateFacingDir()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - controller.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360; // Normalize angle to 0–360

        if (angle >= 337.5f || angle < 22.5f)
        {
            PlayerController.FacingDirection = Directions.Right;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            PlayerController.FacingDirection = Directions.RightUp;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            PlayerController.FacingDirection = Directions.Up;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            PlayerController.FacingDirection = Directions.LeftUp;
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            PlayerController.FacingDirection = Directions.Left;
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            PlayerController.FacingDirection = Directions.LeftDown;
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            PlayerController.FacingDirection = Directions.Down;
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            PlayerController.FacingDirection = Directions.RightDown;
        }
        if (controller.Input.normInputX == 0)
        {
            HandleSpriteFlip(PlayerController.FacingDirection);
        }
    }

    private void HandleSpriteFlip(Directions dir)
    {
        // Flip the parent sprite depending on direction
        switch (dir)
        {
            case Directions.Left:
            case Directions.LeftUp:
            case Directions.LeftDown:
                controller.Parrent.transform.localScale = new Vector3(-1, 1, 1);
                break;

            case Directions.Right:
            case Directions.RightUp:
            case Directions.RightDown:
                controller.Parrent.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }



}

public enum Directions
{
    Right = 0,
    Up = 1,
    Down = 2,
    Left =3,
    RightUp = 4,
    RightDown = 5,
    LeftUp = 6,
    LeftDown = 7,

}
