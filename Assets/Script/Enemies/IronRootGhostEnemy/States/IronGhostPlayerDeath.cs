using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGhostPlayerDeath : IronGhostState
{
    public bool animDone;
    private Coroutine fallbackTimer;

    public IronGhostPlayerDeath(EnemyStateMachine<IronGhostController> stateMachine, IronGhostController controller, string animName)
        : base(stateMachine, controller, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animDone = false;


        controller.OnAnimationDone += Finished;


        fallbackTimer = controller.StartCoroutine(FallbackTimer());
    }

    public override void Exit()
    {
        base.Exit();


        controller.OnAnimationDone -= Finished;


        if (fallbackTimer != null)
        {
            controller.StopCoroutine(fallbackTimer);
            fallbackTimer = null;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (animDone)
        {
            stateMachine.SwitchState(controller.Hide);
        }
    }

    private void Finished()
    {
        animDone = true;
    }

    private IEnumerator FallbackTimer()
    {
        yield return new WaitForSeconds(1f);
        animDone = true;
    }
}
