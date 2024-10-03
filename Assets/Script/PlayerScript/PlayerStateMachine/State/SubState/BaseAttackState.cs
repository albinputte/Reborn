using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackState : ActionState
{
    public BaseAttackState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.weaponAgent.OnExit += HandleExit;
        controller.weaponAgent.Activate(facingDirectionY);

    }

    public void HandleExit()
    {
        IsAbilityDone = true;
    }


   





}
