using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : ActionState
{
    public InteractState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        IInteractable interactable = GetNearestInteractable(2f, controller.InteractionLayer);
        if (interactable != null)
            if (interactable.type == InteractableType.Bush)
                stateMachine.SwitchState(controller.interactBush);
            else if (interactable.type == InteractableType.Minning)
                Debug.Log("Minning");
            //Switch 
            else if (interactable.type == InteractableType.Crafting)
                Debug.Log("Crafting");
                  

    }

}
