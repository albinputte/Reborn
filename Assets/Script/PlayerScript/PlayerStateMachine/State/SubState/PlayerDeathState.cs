using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : ActionState
{
    public PlayerDeathState(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController) : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("hej jag dör");
        controller.OnAnimationEvent += Respawn.instance.MoveCharacterToRespawn;
       controller.OnAnimationDone += () => { IsAbilityDone = true; };
    }

    public override void Exit()
    {
        base.Exit();
        Respawn.instance.RespawnDone();
        PlayerController.FacingDirection[1] = Directions.RightDown;
        controller.OnAnimationEvent -= Respawn.instance.MoveCharacterToRespawn;
        controller.OnAnimationDone -= () => { IsAbilityDone = true; };
    }
}
