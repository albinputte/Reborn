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
        controller.StartCoroutine(SpawnBurnPillars());
        //GameObject.Instantiate(controller.BurnPilar, new Vector3(controller.player.transform.position.x ,controller.player.transform.position.y  so), Quaternion.identity);
        controller.BurnCircle.SetActive(true);
        controller.OnAnimationAction += BurnPilarSpawn;

    }

    private IEnumerator SpawnBurnPillars()
    {
        Vector3 playerPos = controller.player.transform.position;

        // Enable the circle effect
        controller.BurnCircle.SetActive(true);

        // Choose axis (Example: X axis)
        bool useXAxis = Random.value > 0.5f; // change to false if you want Y axis

        // Spawn order offsets
        int[] offsets = { 2, 1, 0, -1, -2 };

        foreach (int offset in offsets)
        {
            Vector3 spawnPos;

            if (useXAxis)
                spawnPos = new Vector3(playerPos.x + offset, playerPos.y, playerPos.z);
            else
                spawnPos = new Vector3(playerPos.x, playerPos.y + offset, playerPos.z);

            GameObject.Instantiate(controller.BurnPilar, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.15f); // small delay
        }
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
