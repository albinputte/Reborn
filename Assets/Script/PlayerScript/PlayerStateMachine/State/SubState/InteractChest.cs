using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : ActionState
{ 
 IInteractable interactable;

    public InteractChest(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
{
    base.Enter();
    interactable = GetNearestInteractable(2f, controller.InteractionLayer);
    if (interactable != null)
        interactable.Interact();
    controller.Input.ActionPefromed = true;
    controller.Input.isInteracting = false;
    controller.OnUiOpen += () => { IsAbilityDone = true; };
}

public override void LogicUpdate()
{
    base.LogicUpdate();
    if (controller.Input.isInteracting == true)
    {
        IsAbilityDone = true;
    }
}
public override void Exit()
{
    base.Exit();
    if (interactable != null)
        interactable.Interact();
    controller.Input.isInteracting = false;
    controller.OnUiOpen -= () => { IsAbilityDone = true; };

}
}
