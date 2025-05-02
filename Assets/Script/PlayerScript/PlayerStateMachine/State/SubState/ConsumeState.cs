using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeState : ActionState
{
    public ConsumeState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.inventoryController.HandleConsumable();
        controller.Input.IsAttacking = false;
        controller.Input.ActionPefromed = false;
        IsAbilityDone = true;
    }

 
}
