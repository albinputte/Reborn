using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBush : ActionState
{
    public InteractBush(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }
    float delay = 0.5f;
    float timer = 0f;
    private bool interactionStarted = false;

    public override void Enter()
    {
        base.Enter();
        IInteractable interactable = GetNearestInteractable(2f, controller.InteractionLayer);
        if (interactable != null)
            interactable.Interact();
        interactionStarted = true;
        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If the interaction started and we're waiting for the delay
        if (interactionStarted)
        {
            timer += Time.deltaTime; // Update the timer

            // If the delay has passed, mark the interaction as complete
            if (timer >= delay)
            {
                IsAbilityDone = true;
            }
        }
    }




    public override void Exit() { base.Exit(); controller.Input.isInteracting = false; controller.Input.ActionPefromed = false; }
}
