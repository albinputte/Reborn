using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostController : EnemyBaseController 
{
    private EnemyStateMachine<StoneGhostController> m_StateMachine;

    public float StartRangeRadius;
    public float LookingRadius;
    public float Speed;
    public Transform GhostTransform;
    public Transform Target;
    public LayerMask PlayerMask;
    public Animator anim;
    public event Action OnAnimationDone, onTakeDamage;
   

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
        Hide = new StoneGhostHide(m_StateMachine, this, "StoneGhost_Burried");
        Looking = new StoneGhostLooking(m_StateMachine, this, "StoneGhost_WakeUp");
        Rising = new StoneGhostRising(m_StateMachine, this, "StoneGhost_Rising");
        NormalChase = new StoneGhostNormalChase(m_StateMachine, this, "StoneGhost_Floating");
        EnterAttack = new StoneGhostEnterAttack(m_StateMachine, this, "StoneGhost_ReadyUp");
        AttackChase = new StoneGhostAttackChase(m_StateMachine, this, "StoneGhost_Charge");
        Hit = new StoneGhostHit(m_StateMachine, this, "StoneGhost_TakeDamage");
        m_StateMachine.InstantiateState(Hide);


    }
    public void OnAnimDone()
    {
        OnAnimationDone?.Invoke();
    }

    public void ontakeDamage()
    {
        onTakeDamage?.Invoke();
    }

    public void Update()
    {
        m_StateMachine.CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        m_StateMachine.CurrentState.PhysicsUpdate();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();

            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 dir = (collision.transform.position - gameObject.transform.position).normalized;
                Debug.Log(damagable);
                if (damagable != null)
                    damagable.Hit(10, dir * 10);
            }
        }

    }
}
