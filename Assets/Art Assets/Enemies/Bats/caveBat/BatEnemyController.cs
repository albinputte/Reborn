
using UnityEngine;

public class BatEnemyController : EnemyBaseController
{
    public EnemyStateMachine<BatEnemyController> StateMachine { get; private set; }

    public BatIdleState IdleState { get; private set; }
    public BatAlertState AlertState { get; private set; }
    public BatPatrolState PatrolState { get; private set; }
    public BatAttackState AttackState { get; private set; }
    public BatFleeState FleeState { get; private set; }
    public BatDeadState DeadState { get; private set; }

    [Header("Detection")]
    public float detectionRadius = 8f;
    public float attackRange = 5f;
    public float fleeRange = 2f;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float fleeSpeed = 7f;
    public float circleRadius = 3f;
    public float circleSpeed = 2f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public GameObject soundWavePrefab;
    public Transform firePoint;

    private float lastAttackTime;
    public float stateTimer;

    public Transform Player;
    public Animator animator;

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine<BatEnemyController>();
        animator = GetComponent<Animator>();
        IdleState   = new BatIdleState(StateMachine, this, "Idle");
        AlertState  = new BatAlertState(StateMachine, this, "Alert");
        PatrolState = new BatPatrolState(StateMachine, this, "Fly");
        AttackState = new BatAttackState(StateMachine, this, "Attack");
        FleeState   = new BatFleeState(StateMachine, this, "Flee");
        DeadState   = new BatDeadState(StateMachine, this, "Dead");
    }

    protected virtual void Start()
    {
        StateMachine.InstantiateState(IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public float DistanceToPlayer =>
        Player == null ? Mathf.Infinity :
        Vector2.Distance(transform.position, Player.position);

    public bool PlayerInDetectionRange() => DistanceToPlayer <= detectionRadius;
    public bool PlayerInAttackRange() => DistanceToPlayer <= attackRange;

    public bool CanAttack =>
        Time.time >= lastAttackTime + attackCooldown;

    public void ResetStateTimer() => stateTimer = 0f;
    public void TickStateTimer() => stateTimer += Time.deltaTime;

    public void FireSoundWave()
    {
        lastAttackTime = Time.time;
        Instantiate(soundWavePrefab, firePoint.position, firePoint.rotation);
    }

    public void CirclePlayer()
    {
        Vector2 toPlayer = (transform.position - Player.position).normalized;
        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);

        Vector2 targetPos =
            (Vector2)Player.position +
            toPlayer * circleRadius +
            perpendicular * Mathf.Sin(Time.time * circleSpeed);

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }
    public static string[] GetAnimNames()
    {
        return new[]
        {
        "Idle",
        "Alert",
        "Fly",
        "Attack",
        "Flee",
        "Dead"
    };
    }
}
