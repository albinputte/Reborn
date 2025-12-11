using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootController : EnemyBaseController
{
    public EnemyStateMachine<GrootController> m_StateMachine;
    public Collider2D[] Collider;
    public Health Health;
    public Animator animator;
    public float MaxHealth;
    public float Healamount;
    public int TimesToheal, TimesToSpawnFirePilar;
    public float[] TimeInBetweenAttack = new float[2];
    public GameObject BurnPilar, BurnCircle;
    public Action OnAnimatioDone,OnAnimationAction;
    public GrootIdle Idle;
    public GrootEnterBurnMode EnterBurn;
    public GrootEnterHealing enterHealing;
    public GrootEnterNormalAttack enterNormlaAttack;
    public GrootHealing Heal;
    public GrootNormalAttack NormalAttack;
    public GrootBurnEnterFireAttack EnterFireAttack;
    public GrootBurnFireAttack FireAttack;
    public GrootBurnEnterFireRangedAttack EnterRangedAttack;
    public GrootBurnFireRangedAttack RangedAttack;
    public GrootBurnIdle BurnIdle;
    public bool BurnMode = false;
    [Header("Movement")]
    public Transform player;                     // Assign in inspector
    public float MoveSpeed = 4f;

    [Header("Line of Sight")]
    public LayerMask obstacleMask;
    private bool hasLOS = false;
    private Vector3 losDir;
    private float losDist;
    // Assign walls/obstacles here

    // A* pathfinding fields
    private List<Node> currentPath = new List<Node>();
    private Node currNode = null;
    private Node EndNode = null;


    public void Start()
    {
        m_StateMachine = new EnemyStateMachine<GrootController>();  
        MaxHealth = Health.GetMaxHealth();
        animator = GetComponent<Animator>();
        EnterBurn = new GrootEnterBurnMode(m_StateMachine, this, "GrootEnterBurnMode");
        enterHealing = new GrootEnterHealing(m_StateMachine, this, "GrootEnterHealing");
        enterNormlaAttack = new GrootEnterNormalAttack(m_StateMachine, this, "GrootEnterNormalAttack");
        Heal = new GrootHealing(m_StateMachine, this, "GrootHealing");
        NormalAttack = new GrootNormalAttack(m_StateMachine, this, "GrootNormalAttack");
        Idle = new GrootIdle(m_StateMachine, this, "GrootIdle");
        BurnIdle = new GrootBurnIdle(m_StateMachine, this, "GrootBurnIdle");
        EnterRangedAttack = new GrootBurnEnterFireRangedAttack(m_StateMachine, this, "GrootBurnEnterRangedAttack");
        RangedAttack = new GrootBurnFireRangedAttack(m_StateMachine, this, "GrootBurnRangedAttack");
        EnterFireAttack = new GrootBurnEnterFireAttack(m_StateMachine, this, "GrootBurnEnterAttack");
        FireAttack = new GrootBurnFireAttack(m_StateMachine, this, "GrootBurnAttack");

       
        m_StateMachine.InstantiateState(Idle);

        SceneManger.instance.OnAllEssentialScenesLoaded += PrepareRefrences;

    }

    public void PrepareRefrences()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        m_StateMachine.CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        m_StateMachine.CurrentState.PhysicsUpdate();
    }

    public void InvokeAnimationDone()
    {
        OnAnimatioDone?.Invoke();
    }

    public void InvokeActionTrigger()
    {
        OnAnimationAction?.Invoke();
    }

    public void EnableAttackColldier()
    {
        Collider[0].enabled = true;
    }

    public void DisableAttackColldier() 
    {
        Collider[0].enabled = false;
    }

    public float GetTime() {  return Time.time; }

    // ---------------------------------------------------------
    //  DISTANCE + MOVEMENT SYSTEM
    // ---------------------------------------------------------

    public float DistanceToPlayer()
    {
        if (player == null)
            return Mathf.Infinity;

        return Vector3.Distance(transform.position, player.position);
    }

    // Main movement (called by AI)
    public void MoveTowardPlayer()
    {
        if (player == null)
            return;

        // If raycast sees the player, use direct movement
        if (HasLineOfSight())
        {
            Debug.Log("Moving Towards");
            MoveDirectlyToPlayer();
        }
        else
        {
            MoveUsingAstar();
        }
    }


    // ---------------------------------------------------------
    //  LINE OF SIGHT CHECK
    // ---------------------------------------------------------

    private bool HasLineOfSight()
    {
        if (player == null) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, player.position);

        losDir = dir;       // store for gizmos
        losDist = dist;

        // If ray hits something → blocked
        if (Physics2D.Raycast(transform.position, dir, dist, obstacleMask))
        {
            hasLOS = false;
            return false;
        }

        hasLOS = true;
        return true;
    }


    // ---------------------------------------------------------
    // DIRECT MOVEMENT (no obstacles)
    // ---------------------------------------------------------

    private void MoveDirectlyToPlayer()
    {
        if (player == null)
            return;

        Vector3 dir = (player.position - transform.position).normalized;

        // Move using MoveTowards (cleaner + more stable)
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            MoveSpeed * Time.deltaTime
        );

       
        // 2D sprite/mesh flip
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    // ---------------------------------------------------------
    // ASTAR MOVEMENT (when line-of-sight is blocked)
    // ---------------------------------------------------------

    private void MoveUsingAstar()
    {
        float speed = MoveSpeed;

        // Auto-reset path if target node changes
        if (AstarManger.instance.FindNearestNode(player.position) != EndNode)
        {
            currentPath.Clear();
        }

        // Get the node we are currently on
        if (currNode == null)
        {
            currNode = AstarManger.instance.FindNearestNode(transform.position);
        }

        // Follow existing path
        if (currentPath.Count > 0)
        {
            int x = 0;

            Vector3 targetPos = new Vector3(
                currentPath[x].transform.position.x,
                currentPath[x].transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, currentPath[x].transform.position) < 0.1f)
            {
                currNode = currentPath[x];
                currentPath.RemoveAt(x);
            }
        }
        else
        {
            // Need a new path
            currentPath = AstarManger.instance.GeneratePath(
                currNode,
                EndNode = AstarManger.instance.FindNearestNode(player.position)
            );

            // Skip starting node if possible
            if (currentPath.Count > 1)
            {
                currNode = currentPath[1];
                currentPath.RemoveAt(0);
            }
        }

        // 2D flip
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = hasLOS ? Color.green : Color.red;

        Gizmos.DrawLine(
            transform.position,
            transform.position + losDir * losDist
        );

        Gizmos.DrawSphere(
            transform.position + losDir * losDist,
            0.1f
        );
    }
}
