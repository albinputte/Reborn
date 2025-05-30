using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMinning : ActionState
{

    IInteractable interactable;

    public InteractMinning(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Input.ActionPefromed = true;
        interactable = GetNearestInteractable(1f, controller.InteractionLayer);
        controller.OnAnimationDone +=  EndMine;
        controller.OnAnimationEvent += Mine;

    }


    public void Mine()
    {
        if (interactable != null)
        {
            Debug.Log("hej");
            interactable.Interact();
            CameraShake.instance.ShakeCamera(2f, 0.3f);
          
        }
           
    }

    public void EndMine()
    {
        IsAbilityDone = true;
    }



    public override void Exit()
    {
        base.Exit();
        controller.OnAnimationEvent -= Mine;
        controller.OnAnimationDone -= EndMine;
        if (controller.Input.IsAttacking) {
            controller.Input.IsAttacking  = false;
            controller.Input.ActionPefromed = false;
        }



        controller.Input.isInteracting = false;

    }
}
