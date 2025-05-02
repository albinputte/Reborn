using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostController : EnemyBaseController 
{
   public EnemyStateMachine<StoneGhostController> m_StateMachine;

    public float StartRangeRadius;
    public float LookingRadius;
    public float Speed;
    public Transform GhostTransform;
    public Transform Target;
    public LayerMask PlayerMask;
    public event Action OnAnimationDone;

    public StoneGhostHide Hide {  get; set; }  
    public StoneGhostLooking Looking { get; set; }
    public StoneGhostRising Rising { get; set; }
    public StoneGhostNormalChase NormalChase { get; set; }
    public StoneGhostEnterAttack EnterAttack { get; set; }
    public StoneGhostAttackChase AttackChase { get; set; }
    public StoneGhostHit Hit { get; set; }

    public void Start()
    {
        m_StateMachine = new EnemyStateMachine<StoneGhostController>();
        Hide = new StoneGhostHide(m_StateMachine, this, "Hide");
        Looking = new StoneGhostLooking(m_StateMachine, this, "Looking");
        Rising = new StoneGhostRising(m_StateMachine, this, "Rise");
        NormalChase = new StoneGhostNormalChase(m_StateMachine, this, "NormalChase");
        EnterAttack = new StoneGhostEnterAttack(m_StateMachine, this, "EnterAttack");
        AttackChase = new StoneGhostAttackChase(m_StateMachine, this, "AttackChase");
        Hit = new StoneGhostHit(m_StateMachine, this, "Hit");

    }

    public void Update()
    {
        m_StateMachine.CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        m_StateMachine.CurrentState.PhysicsUpdate();
    }
}
