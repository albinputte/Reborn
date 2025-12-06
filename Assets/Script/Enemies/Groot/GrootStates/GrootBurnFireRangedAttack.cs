using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootBurnFireRangedAttack : GrootState
{
    public GrootBurnFireRangedAttack(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

    public bool animDone;
    public int TimesSpawnerPilar;


    public override void Enter()
    {
        base.Enter();
        animDone = false;
        TimesSpawnerPilar = 0;
        //add knockback
        controller.OnAnimationAction += BurnPilarSpawn;
        controller.OnAnimatioDone += FinishAnimation;

    }

    public override void Exit()
    {
        IdleTime = controller.GetTime() + Random.Range(controller.TimeInBetweenAttack[0], controller.TimeInBetweenAttack[1]);
        base.Exit();
        IsPeforminAction = false;
        controller.OnAnimationAction -= BurnPilarSpawn;
        controller.OnAnimatioDone -= FinishAnimation;
        controller.BurnCircle.SetActive(false);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (animDone)
        {
            stateMachine.SwitchState(controller.BurnIdle);
        }
    }

    public void BurnPilarSpawn()
    {

        controller.OnAnimationAction -= BurnPilarSpawn;
        GameObject.Instantiate(controller.BurnPilar, new Vector3(controller.player.transform.position.x ,controller.player.transform.position.y ), Quaternion.identity);
        controller.BurnCircle.SetActive(true);
        controller.OnAnimationAction += BurnPilarSpawn;

    }
    public void FinishAnimation()
    {
        TimesSpawnerPilar++;
        
        if (TimesSpawnerPilar == controller.TimesToSpawnFirePilar)
        {
            animDone = true;
        }
          
    }

}
