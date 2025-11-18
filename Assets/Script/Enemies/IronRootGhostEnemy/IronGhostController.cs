using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGhostController : EnemyBaseController
{
    private EnemyStateMachine<IronGhostController> m_StateMachine;

    public float StartRangeRadius;
    public float LookingRadius;
    public float Speed;
    public Transform GhostTransform;
    public Transform Target;
    public LayerMask PlayerMask;
    public Animator anim;
    public event Action OnAnimationDone, onTakeDamage, onSearch, OnHitPlayer, OnPlayAudio;
    public Health health;
    public LayerMask Layer;
    private Rigidbody2D rb;
    public Collider2D col;
    [SerializeField] private GameObject HitPrefab;
    [SerializeField] private bool isConquestEnemy = false;

    // Gör om så att animationene "rising" tas bort och 
    //att wake up byter mid animation wid animation trigger till go down om spelaren ej är close
    public IronGhostHide Hide { get; set; }
    public IronGhostLooking Looking { get; set; }
    public IronGhostRising Rising { get; set; }
    public IronGhostNormalChase NormalChase { get; set; }
    public IronGhostEnterAttack EnterAttack { get; set; }
    public IronGhostAttackChase AttackChase { get; set; }
    public IronGhostHit Hit { get; set; }
    public IronGhostDeathState DeathState { get; set; }

    public IronGhostPlayerDeath PlayerDied { get; set; }

    public void Start()
    {
        m_StateMachine = new EnemyStateMachine<IronGhostController>();
        Hide = new IronGhostHide(m_StateMachine, this, "StoneGhost_Burried");
        Looking = new IronGhostLooking(m_StateMachine, this, "StoneGhost_WakeUp");
        Rising = new IronGhostRising(m_StateMachine, this, "StoneGhost_Rising");
        NormalChase = new IronGhostNormalChase(m_StateMachine, this, "StoneGhost_Floating");
        EnterAttack = new IronGhostEnterAttack(m_StateMachine, this, "StoneGhost_ReadyUp");
        AttackChase = new IronGhostAttackChase(m_StateMachine, this, "StoneGhost_Charge");
        Hit = new IronGhostHit(m_StateMachine, this, "StoneGhost_TakeDamage");
        DeathState = new IronGhostDeathState(m_StateMachine, this, "StoneGhost_Die");
        PlayerDied = new IronGhostPlayerDeath(m_StateMachine, this, "StoneGhostPlayerDead");
        if (isConquestEnemy)
        {
            m_StateMachine.InstantiateState(Rising);
        }
        else
        {
            m_StateMachine.InstantiateState(Hide);
        }
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        health.OnDeath.AddListener(() => m_StateMachine.SwitchState(DeathState));
        Target = GameObject.FindWithTag("Player").transform;
    }
    public void OnAnimDone()
    {
        OnAnimationDone?.Invoke();
    }

    public void ontakeDamage()
    {
        onTakeDamage?.Invoke();
    }
    public void SearchForPlayer()
    {
        onSearch?.Invoke();
    }
    public void OnAudioInvoke()
    {
        OnPlayAudio?.Invoke();
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
                {
                    OnHitPlayer?.Invoke();
                    SoundManager.PlaySound(SoundType.StoneGhost_Hit);
                    damagable.Hit(10, dir * 10);
                    health.TakeKnockBack(rb, (dir * 10) * -1);
                    health.SpawnParticlesFromTarget(HitPrefab, (dir * 10));
                    CameraShake.instance.ShakeCamera(2f, 0.3f);

                }

            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, LookingRadius);
        Gizmos.DrawWireSphere(transform.position, StartRangeRadius);
    }

}

