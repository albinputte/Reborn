using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected string animationName;
    protected PlayerController controller;
    protected Vector2 CurrentVelocity;
    protected bool IsAbilityDone;
    protected Directions directions;
    private float checkCooldown = 0f;
    private const float checkInterval = 0.1f;
    private static readonly Dictionary<Vector2, Directions> DirectionMap = new Dictionary<Vector2, Directions>
{
    { new Vector2(1, 0), Directions.Right },
    { new Vector2(0, 1), Directions.Up },
    { new Vector2(0, -1), Directions.Down },
    { new Vector2(-1, 0), Directions.Left },
    { new Vector2(1, 1), Directions.RightUp },
    { new Vector2(1, -1), Directions.RightDown },
    { new Vector2(-1, 1), Directions.LeftUp },
    { new Vector2(-1, -1), Directions.LeftDown }
};

    private static readonly (float min, float max, Directions dir)[] angleToDirectionMap = new[]
 {
    (337.5f, 360f, Directions.Right),
    (0f, 22.5f, Directions.Right),
    (22.5f, 67.5f, Directions.RightUp),
    (67.5f, 112.5f, Directions.Up),
    (112.5f, 157.5f, Directions.LeftUp),
    (157.5f, 202.5f, Directions.Left),
    (202.5f, 247.5f, Directions.LeftDown),
    (247.5f, 292.5f, Directions.Down),
    (292.5f, 337.5f, Directions.RightDown)
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
        checkCooldown -= Time.deltaTime;
        if (checkCooldown <= 0f)
        {
            CheckIfInteractionIsNear();
            checkCooldown = checkInterval;
        }
        CurrentVelocity = controller.rb.velocity;
        Debug.Log(stateMachine.CurrentState);
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

        if (DirectionMap.TryGetValue(faceDirCheck, out Directions dir))
        {
            PlayerController.FacingDirection[1] = dir;
        }
    }

    public Directions CalculateFacingDir(bool ActionCalc)
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - controller.transform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f) % 360f;

        foreach (var (min, max, dir) in angleToDirectionMap)
        {
            if ((angle >= min && angle < max) || (min > max && (angle >= min || angle < max))) // handles wrapping
            {
                //HandleSpriteFlip(dir);
                return dir;
            }
        }

        return Directions.RightDown; // fallback
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


    public IInteractable GetNearestInteractable(float detectRadius, LayerMask interactableLayer)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, detectRadius, interactableLayer);
        IInteractable nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                float dist = Vector2.Distance(controller.transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = interactable;
                }
            }
        }

        return nearest;
    }
    public void CheckIfInteractionIsNear()
    {
        var nearest = GetNearestInteractable(1f, controller.InteractionLayer);

        if (nearest != null)
        {
            if (controller.CurrentNearest == null)
            {
                controller.CurrentNearest = nearest;
                controller.CurrentNearest.NearPlayer();
            }
            else if (controller.CurrentNearest != nearest)
            {
                controller.CurrentNearest.LeavingPlayer();
                controller.CurrentNearest = nearest;
                controller.CurrentNearest.NearPlayer();
            }

        }
        else
        {
            if (controller.CurrentNearest != null)
                controller.CurrentNearest.LeavingPlayer();
            controller.CurrentNearest = null;
        }
           
       
    }

    protected void HandleAttackInput()
    {
        if (!controller.Input.IsAttacking) return;

        if (!InventoryController.NoWeaponEquiped)
        {
            stateMachine.SwitchState(controller.baseAttack);
        }
        else if (InventoryController.IsConsumableEquiped || InventoryController.IsAccesoireInHand)
        {
            stateMachine.SwitchState(controller.consumeState);
        }
        else
        {
            controller.Input.IsAttacking = false;
            controller.Input.ActionPefromed = false;
        }
    }

    protected void HandleMovementInput()
    {
        var inputX = controller.Input.normInputX;
        var inputY = controller.Input.normInputY;

        if (inputX == 0 && inputY == 0)
        {
            stateMachine.SwitchState(controller.idle);
        }
        else if (controller.Input.isSprinting)
        {
            stateMachine.SwitchState(controller.run);
        }
        else
        {
            stateMachine.SwitchState(controller.move);
        }
    }

    protected void HandleInteractionInput()
    {
        if (!controller.Input.isInteracting) return;

        if (GetNearestInteractable(1f, controller.InteractionLayer) != null)
        {
            stateMachine.SwitchState(controller.interactState);
        }
        else
        {
            controller.Input.isInteracting = false;
            controller.Input.ActionPefromed = false;
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
