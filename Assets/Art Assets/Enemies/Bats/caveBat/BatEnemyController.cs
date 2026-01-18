
using System.Collections.Generic;
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

    [Header("Flee")]
    public float fleeDuration = 1.5f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public GameObject soundWavePrefab;
    public Transform firePoint;

    private float lastAttackTime;
    public float stateTimer;

    public Transform Player;
    public Animator animator;

    [Header("Pathfinding")]
    public List<Node> currentPath = new List<Node>();
    public Node currNode;
    public Node endNode;
    [Header("Path Repathing")]
    public float repathInterval = 0.25f; // seconds
    private float lastRepathTime;

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine<BatEnemyController>();
        animator = GetComponent<Animator>();

        IdleState = new BatIdleState(StateMachine, this, "Idle");
        AlertState = new BatAlertState(StateMachine, this, "Alert");
        PatrolState = new BatPatrolState(StateMachine, this, "Fly");
        AttackState = new BatAttackState(StateMachine, this, "Attack");
        FleeState = new BatFleeState(StateMachine, this, "Flee");
        DeadState = new BatDeadState(StateMachine, this, "Dead");
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

        GameObject wave =
            Instantiate(soundWavePrefab, firePoint.position, Quaternion.identity);

        Vector2 dir = (Player.position - firePoint.position).normalized;
        wave.transform.up = dir;
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

    public void FleeFromPlayerPathfinding(float speed)
    {

        float move = speed * Time.deltaTime;

        // 🔒 Only repath occasionally
        if (Time.time - lastRepathTime > repathInterval)
        {
            lastRepathTime = Time.time;

            Node newEndNode = AstarManger.instance.FindFurthestNode(
                Player.position,
                transform.position
            );

            if (newEndNode != endNode)
            {
                endNode = newEndNode;
                currNode = AstarManger.instance.FindNearestNode(transform.position);
                currentPath = AstarManger.instance.GeneratePath(currNode, endNode);
            }
        }

        if (currentPath == null || currentPath.Count == 0)
            return;

        Node nextNode = currentPath[0];

        Vector3 targetPos = new Vector3(
            nextNode.transform.position.x,
            nextNode.transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(transform.position, targetPos, move);

        if (Vector2.Distance(transform.position, nextNode.transform.position) < 0.1f)
        {
            currNode = nextNode;
            currentPath.RemoveAt(0);
        }
        else
        {
            // Generate new flee path
            currentPath.Clear() ;
            currentPath = AstarManger.instance.GeneratePath(currNode, endNode);

            if (currentPath != null && currentPath.Count > 1)
            {
                currNode = currentPath[1];
                currentPath.RemoveAt(0);
            }
        }

        // Optional facing logic
        if (Player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

}
