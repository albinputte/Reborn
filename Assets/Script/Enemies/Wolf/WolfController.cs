using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WolfController : EnemyBaseController
{
    public EnemyStateMachine<WolfController> stateMachine;
    public Health health;
    public Node HomeNode = null;
    [SerializeField] public List<Node> currentPath = new List<Node>();
    private Node currNode = null;
    private Node EndNode = null;
    public List<Node> PatrolableNodes = new List<Node>();
    public Animator animator;
    public bool CanPatrol = false;
    public bool IsPatrolling = false;
    public bool PatrolDone;
    public Action OnAnimationDone, OnAnimationActionTrigger,OnTakeDamage;
    public Collider2D AttackCollider;
    public Collider2D LOS;
    public bool iSePlayer = false;

    public int[] PatrolBounds = new int[4];
    public float speed;
    public float runSpeed;
    public float[] IdleTime = new float[2];
    public Transform player;
    public LayerMask obstacleMask;
    public WolfChase chase;
    public WolfDeath death;
    public WolfHit hit;
    public WolfLungeAttack attack;
    public WolfPatrol patrol;
    public WolfPatrolSpotted spotted;
    public WolfReturnToTerritory ReturnToTerritory;
    public WoflIdle idle;
    public enum ActionType { None, Chase, Attack, Patrol, ReturnToTerritory }

    public ActionType chosenAction = ActionType.None;
    void Start()
    {
        stateMachine = new EnemyStateMachine<WolfController> ();

        chase = new WolfChase(stateMachine,this, "WolfChase");
        death = new WolfDeath(stateMachine, this, "WolfDeath");
        hit = new WolfHit(stateMachine, this, "WolfHit");
        attack = new WolfLungeAttack(stateMachine, this, "WolfBite");
        patrol = new WolfPatrol(stateMachine, this, "WolfWalk");
        spotted = new WolfPatrolSpotted(stateMachine, this, "WolfSpottedPlayer");
        ReturnToTerritory = new WolfReturnToTerritory(stateMachine, this, "WolfWalk");
        idle = new WoflIdle(stateMachine, this, "WolfIdle");
        animator = GetComponent<Animator> ();
        stateMachine.InstantiateState(patrol);
        FindPatrollableNode();
        health = GetComponent<Health> ();
        health.OnDeath.AddListener(() => stateMachine.SwitchState(death));
        SceneManger.instance.OnAllEssentialScenesLoaded += PrepareRefrences;
      

    }

   

    public void PrepareRefrences()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    public void InvokeAnimationDone()
    {
        OnAnimationDone?.Invoke();
    }

    public void InvokeActionTrigger()
    {
        OnAnimationActionTrigger?.Invoke();
    }


    void Update()
    {
      stateMachine.CurrentState.LogicUpdate();
    }

    public void DeactivateAttackCollider()
    {
        AttackCollider.enabled = false;
    }
    public void ActivateAttackCollider()
    {
        AttackCollider.enabled = true;
    }
    public void InvokeTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }

  


    public void FindPatrollableNode()
    {
        HomeNode = AstarManger.instance.FindNearestNode(transform.position);

        if (HomeNode == null)
        {
            Invoke(nameof(FindPatrollableNode), 0.5f);
            return;
        }

        // Scan all nodes inside patrol bounds
        for (int y = PatrolBounds[2]; y <= PatrolBounds[3]; y++)
        {
            for (int x = PatrolBounds[0]; x <= PatrolBounds[1]; x++)
            {
                Vector3 checkPos = new Vector3(HomeNode.transform.position.x + x, HomeNode.transform.position.y + y, 0);

                Node TempNode = AstarManger.instance.FindNearestNode(checkPos);

                if (TempNode != null && Vector3.Distance(TempNode.transform.position, checkPos) < 0.1f)
                {
                    PatrolableNodes.Add(TempNode);
                }
            }
        }

        Debug.Log("Patrollable Nodes: " + PatrolableNodes.Count);

        CanPatrol = PatrolableNodes.Count > 0;
       
    }
    public float GetDistanceBetweenEnemyAndObject(Transform ObjectToCompare)
    {
        if (player == null)
            return Mathf.Infinity;

        return Vector3.Distance(transform.position, ObjectToCompare.position);
    }

    public void FindPatrol()
    {
        currNode = AstarManger.instance.FindNearestNode(transform.position);

        if (currNode == null)
        {
            Invoke(nameof(FindPatrol), 0.5f);
            return;
        }

        int loopCount = 0;
        bool foundPath = false;

        while (!foundPath)
        {
            // Pick random patrol node
            Node randomTarget = PatrolableNodes[UnityEngine.Random.Range(0, PatrolableNodes.Count)];

            EndNode = randomTarget;
            currentPath = AstarManger.instance.GeneratePath(currNode, EndNode);

            if (currentPath != null && currentPath.Count >= 3)
            {
                foundPath = true;

                // skip starting node
                currNode = currentPath[1];
                currentPath.RemoveAt(0);
            }

            loopCount++;

            if (loopCount >= 25)
            {
                // Go back home if no good patrol route found
                currentPath = AstarManger.instance.GeneratePath(currNode, HomeNode);
                foundPath = true;
            }
        }
        IsPatrolling = true;
    }


    public void Patrol()
    {
        if (!CanPatrol)
            return;

        if (currentPath.Count > 0)
        {
            Vector3 targetPos = new Vector3(
                currentPath[0].transform.position.x,
                currentPath[0].transform.position.y,
                transform.position.z
            );
            Vector3 moveDir = targetPos - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                currNode = currentPath[0];
                currentPath.RemoveAt(0);
            }
            FlipByMovement(moveDir);
        }
        else
        {
            if (!IsPatrolling)
            {
                FindPatrol();
            }
            else {
            PatrolDone = true;
            }
        }

      

    }
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

       

        // If ray hits something → blocked
        if (Physics2D.Raycast(transform.position, dir, dist, obstacleMask))
        {
      
            return false;
        }

    
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
            runSpeed * Time.deltaTime
        );


        FlipByMovement(dir);
    }
    private void FlipByMovement(Vector3 direction)
    {
        if (direction.x > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        else if (direction.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    // ---------------------------------------------------------
    // ASTAR MOVEMENT (when line-of-sight is blocked)
    // ---------------------------------------------------------

    private void MoveUsingAstar()
    {


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
            Vector3 moveDir = targetPos - transform.position;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                runSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, currentPath[x].transform.position) < 0.1f)
            {
                currNode = currentPath[x];
                currentPath.RemoveAt(x);
            }
            FlipByMovement(moveDir);
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

    }

    public void MoveToHomeNode()
    {


        // Auto-reset path if target node changes
        if (AstarManger.instance.FindNearestNode(HomeNode.transform.position) != EndNode && EndNode == HomeNode)
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
            Vector3 moveDir = targetPos - transform.position;

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
            FlipByMovement(moveDir);
        }
        else
        {
            // Need a new path
            currentPath = AstarManger.instance.GeneratePath(
                currNode,
                EndNode = AstarManger.instance.FindNearestNode(HomeNode.transform.position)
            );

            // Skip starting node if possible
            if (currentPath.Count > 1)
            {
                currNode = currentPath[1];
                currentPath.RemoveAt(0);
            }
        }

    }

    public void StartLunge()
    {
        StartCoroutine(LungeTowardsPlayer(4,0.2f));
    }

    public IEnumerator LungeTowardsPlayer(float lungeSpeed, float lungeDuration)
    {
        if (player == null)
            yield break;

        Vector3 direction = (player.position - transform.position).normalized;

        float timer = 0f;

        while (timer < lungeDuration)
        {
            transform.position += direction * lungeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
