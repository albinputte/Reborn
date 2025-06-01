using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMinning : ActionState
{
    private IInteractable interactable;
    private float abilityDuration = 0.5f; // Duration of animation in seconds
    private float timer;

    public InteractMinning(PlayerStateMachine StateMachine, PlayerData data, string animName, PlayerController playerController)
        : base(StateMachine, data, animName, playerController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Input.ActionPefromed = true;
        interactable = GetNearestInteractable(1f, controller.InteractionLayer);

        controller.OnAnimationDone += OnAnimDone;
        controller.OnAnimationEvent += Mine;

        timer = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Timer fallback
        timer += Time.deltaTime;
        if (timer >= abilityDuration)
        {
            IsAbilityDone = true;
        }

        Debug.Log(IsAbilityDone);
    }

    private void OnAnimDone()
    {
        IsAbilityDone = true;
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

    public override void Exit()
    {
        controller.OnAnimationDone -= OnAnimDone;
        controller.OnAnimationEvent -= Mine;
        base.Exit();

        Debug.Log("i Exit");
        controller.Input.isInteracting = false;

        if (controller.Input.IsAttacking)
        {
            controller.Input.IsAttacking = false;
            controller.Input.ActionPefromed = false;
        }
    }
}
