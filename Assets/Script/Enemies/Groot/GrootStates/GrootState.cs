using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootState : BaseEnemyState<GrootController>
{
    public GrootState(EnemyStateMachine<GrootController> stateMachine, GrootController controller, string animName) : base(stateMachine, controller, animName)
    {
    }



    public bool IsPeforminAction;
    public bool IfNearPlayer = true;

    public static bool MandatoryHeal = true;
    public static bool BurnMode = false;

    public static float IdleTime;

    //-------------------------------------------------------
    // NEW BRAIN INTERNAL STATE
    //-------------------------------------------------------
    private enum ActionType { None, NormalMelee, NormalRanged, BurnMelee, BurnRanged, Heal }

    private ActionType chosenAction = ActionType.None;
    private bool actionInProgress = false;

    private float meleeRange = 1.5f;
    private float rangedPreferredDistance = 7f;


    //-------------------------------------------------------

    public override void Enter()
    {
        base.Enter();
        controller.animator.Play(animName);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Switch to burn mode at 50% HP
        if (!IsPeforminAction && controller.Health.GetCurrentHealth() <= controller.MaxHealth / 2 && !BurnMode)
        {
            BurnMode = true;
            controller.m_StateMachine.SwitchState(controller.EnterBurn);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    // ====================================================================
    // NORMAL MODE BRAIN (Now Uses Stored Actions + Movement Before Melee)
    // ====================================================================

    public void BrainNormalMode()
    {
        float distance = controller.DistanceToPlayer();

        if (controller.GetTime() < IdleTime)
            return;

        if (!IfNearPlayer)
            return;

        //-------------------------------------------------------
        // ❗ If we already picked an action, continue that one
        //-------------------------------------------------------
        if (actionInProgress)
        {
            ContinueAction_Normal(distance);
            return;
        }

        //-------------------------------------------------------
        // ❗ Action selection (only happens once)
        //-------------------------------------------------------
        // Mandatory heal if HP <= 70% and hasn't healed yet
        if (controller.Health.GetCurrentHealth() <= controller.MaxHealth * 0.8f && MandatoryHeal)
        {
            MandatoryHeal = false;
            chosenAction = ActionType.Heal;
            actionInProgress = true;
            ContinueAction_Normal(distance);
            return;
        }

        float random = Random.Range(0f, 1f);

        // Heal chance if HP <= 90%
        if (random >= 0.8f && controller.Health.GetCurrentHealth() <= controller.MaxHealth * 0.9f)
        {
            chosenAction = ActionType.Heal;
        }
        else
        {
            // Weight: If far → prefer ranged
            bool preferRanged = distance > meleeRange * 2f;
            bool chooseRanged = preferRanged ? (random < 0.75f) : (random < 0.3f);

            chosenAction = ActionType.NormalMelee;
        }

        actionInProgress = true;
        ContinueAction_Normal(distance);
    }



    // ====================================================================
    // Continue logic for normal mode (persistent action)
    // ====================================================================

    private void ContinueAction_Normal(float distance)
    {
        switch (chosenAction)
        {
            case ActionType.Heal:
                controller.m_StateMachine.SwitchState(controller.enterHealing);
                ResetAction();
                break;

            case ActionType.NormalRanged:
                // If too close, try to get some space
               

                controller.m_StateMachine.SwitchState(controller.enterNormlaAttack);
                ResetAction();
                break;

            case ActionType.NormalMelee:
                // Must move toward player if too far
                if (distance > meleeRange)
                {
                    controller.MoveTowardPlayer();
                    return;
                }

                controller.m_StateMachine.SwitchState(controller.enterNormlaAttack);
                ResetAction();
                break;
        }
    }



    // ====================================================================
    // BURN MODE BRAIN (integrated like you requested)
    // ====================================================================

    public void BrainBurnMode()
    {
        if (controller.GetTime() < IdleTime)
            return;

        float distance = controller.DistanceToPlayer();

        //-------------------------------------------------------
        // Continue already chosen action
        //-------------------------------------------------------
        if (actionInProgress)
        {
            ContinueAction_Burn(distance);
            return;
        }

        //-------------------------------------------------------
        // Choose new action
        //-------------------------------------------------------
        float random = Random.Range(0f, 1f);

        bool preferRanged = distance > meleeRange * 3f;
        bool chooseRanged = preferRanged ? (random < 0.75f) : (random < 0.30f);

        chosenAction = chooseRanged ? ActionType.BurnRanged : ActionType.BurnMelee;
        actionInProgress = true;

        ContinueAction_Burn(distance);
    }



    // ====================================================================
    // Burn action continuation
    // ====================================================================

    private void ContinueAction_Burn(float distance)
    {
        switch (chosenAction)
        {
            case ActionType.BurnRanged:

           

                controller.m_StateMachine.SwitchState(controller.EnterRangedAttack);
                ResetAction();
                break;


            case ActionType.BurnMelee:

                // Move closer if needed
                if (distance > meleeRange)
                {
                    controller.MoveTowardPlayer();
                    return;
                }

                controller.m_StateMachine.SwitchState(controller.EnterFireAttack);
                ResetAction();
                break;
        }
    }



    // ====================================================================
    // RESET ACTION
    // ====================================================================

    private void ResetAction()
    {
        chosenAction = ActionType.None;
        actionInProgress = false;
        IdleTime = controller.GetTime() + 1f;  // cooldown
    }
}


