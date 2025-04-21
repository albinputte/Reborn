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
    protected Vector2[] FaceDir =
    {
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, 1),
        new Vector2(-1, -1),
    }; 
    
   


    public PlayerState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController ) {
        stateMachine = StateMachine;
        playerData = data;
        animationName = animName;
        controller = playerController;
    
    }

    public virtual void Enter()
    {

        controller.animator.Play(animationName + "_" + (int)PlayerController.FacingDirection[1]);  //add later when i add player controller script
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

  

    public void ResetFlip()
    {
            controller.Parrent.transform.localScale = new Vector3(1, 1, 0);
    }

    public void HandleFacingDirection()
    {
        Vector2 faceDirCheck = new Vector2(controller.Input.normInputX, controller.Input.normInputY);
        int Dir = -1;
        for (int i = 0; i < FaceDir.Length; i++)
        {
            if(faceDirCheck == FaceDir[i])
                Dir = i;
        }
        switch(Dir)
        {
            case 0:
                PlayerController.FacingDirection[1] = Directions.Right; break;
            case 1:
                PlayerController.FacingDirection[1] = Directions.Up; break;
            case 2:
                PlayerController.FacingDirection[1] = Directions.Down; break;
            case 3:
                PlayerController.FacingDirection[1] = Directions.Left; break;
            case 4:
                PlayerController.FacingDirection[1] = Directions.RightUp; break;
            case 5:
                PlayerController.FacingDirection[1] = Directions.RightDown; break;
            case 6:
                PlayerController.FacingDirection[1] = Directions.LeftUp; break;
            case 7:
                PlayerController.FacingDirection[1] = Directions.LeftDown; break;
            default:
                break;
        }
      

        
    }

    public Directions CalculateFacingDir(bool ActionCalc)
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - controller.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360; // Normalize angle to 0–360

        if (angle >= 337.5f || angle < 22.5f)
        {
            HandleSpriteFlip(Directions.Right);
           return Directions.Right;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            HandleSpriteFlip(Directions.RightUp);
            return Directions.RightUp;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            HandleSpriteFlip(Directions.Up);
            return Directions.Up;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            HandleSpriteFlip(Directions.LeftUp);
            return Directions.LeftUp;
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            HandleSpriteFlip(Directions.Left);
            return Directions.Left;
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            HandleSpriteFlip(Directions.LeftDown);
            return Directions.LeftDown;
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            HandleSpriteFlip(Directions.Down);
            return Directions.Down;
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            HandleSpriteFlip(Directions.RightDown);
            return Directions.RightDown;
        }
        else { return 0; }
      
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
