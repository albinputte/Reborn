using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneGhostState : BaseEnemyState<StoneGhostController>
{
    public static int WasAttacked;
    public static bool IsDamaged = false;
    private List<Node> currentPath = new List<Node>();
    private Node currNode;
    private Node EndNode;
    private int pathIndex = 0;
    public StoneGhostState(EnemyStateMachine<StoneGhostController> stateMachine, StoneGhostController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        controller.anim.Play(animName);
        controller.onTakeDamage += Hit;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
  
        if (Respawn.instance.isRespawning)
        {
            if(stateMachine.CurrentState != controller.Hide || stateMachine.CurrentState != controller.PlayerDied)
            {
                stateMachine.SwitchState(controller.PlayerDied);
            }
               
        }

       
    }
       

    private float randomMoveTimer = 0f;          // Timer for random movement
    private float randomMoveDuration = 0.5f;     // Duration for random movement in one direction
    private bool isRandomMoving = false;         // Flag to track if random movement is active
    private Vector3 currentRandomDirection = Vector3.zero;  // Store the current random direction during random movement

    protected void RaycastMovement(float speed, Transform transform, Transform player, bool prefersToChase)
    {
        Vector3[] directions = {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        new Vector3(1, 1).normalized,
        new Vector3(1, -1).normalized,
        new Vector3(-1, 1).normalized,
        new Vector3(-1, -1).normalized,
        new Vector3(0.5f, 1).normalized,
        new Vector3(1, 0.5f).normalized,
        new Vector3(0.5f, -1).normalized,
        new Vector3(-1, 0.5f).normalized,
    };

        float moveSpeed = speed * Time.deltaTime;
        Vector3 bestDirection = Vector3.zero;
        float bestScore = -Mathf.Infinity;
        float tooCloseDistance = 3f;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool foundValidDirection = false;

        List<Vector3> validDirections = new List<Vector3>();
        foreach (var dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 5f, controller.Layer);
            if (hit.collider == null)
            {
                validDirections.Add(dir);
            }
        }

        // If not close and not in random movement
        if (distanceToPlayer > tooCloseDistance && !isRandomMoving)
        {
           // 50% chance to chase vs flee

            foreach (var direction in validDirections)
            {
                float score = Vector3.Distance(transform.position + direction * 5f, player.position);

                // Flip scoring based on chase or flee
                if (prefersToChase)
                {
                    score = -score; // lower distance is better
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = direction;
                    foundValidDirection = true;
                }
            }

            if (foundValidDirection)
            {
                Vector3 targetPosition = transform.position + bestDirection * 5f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);
            }
            else
            {
                Move(speed, transform, player); // fallback
            }
        }
        // If too close, begin random movement away from player
        else if (distanceToPlayer <= tooCloseDistance && !isRandomMoving)
        {
            isRandomMoving = true;

            if (validDirections.Count > 0)
            {
                List<Vector3> awayFromPlayerDirs = new List<Vector3>();
                Vector3 toPlayer = (player.position - transform.position).normalized;

                foreach (var dir in validDirections)
                {
                    float dot = Vector3.Dot(dir, toPlayer);
                    if (dot < 0) // Points away from player
                    {
                        awayFromPlayerDirs.Add(dir);
                    }
                }

                if (awayFromPlayerDirs.Count > 0)
                {
                    currentRandomDirection = awayFromPlayerDirs[Random.Range(0, awayFromPlayerDirs.Count)];
                }
                else
                {
                    currentRandomDirection = validDirections[Random.Range(0, validDirections.Count)];
                }
            }
            else
            {
                currentRandomDirection = Vector3.zero;
            }

            randomMoveTimer = randomMoveDuration;
        }

        // Handle ongoing random movement
        if (isRandomMoving)
        {
            if (randomMoveTimer <= 0f)
            {
                isRandomMoving = false;
                currentRandomDirection = Vector3.zero;
            }
            else
            {
                Vector3 targetPosition = transform.position + currentRandomDirection * 5f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);
                randomMoveTimer -= Time.deltaTime;
            }
        }

        FlipEnemy(transform, player);
    }



    // Method to flip the enemy based on player's position
    private void FlipEnemy(Transform transform, Transform player)
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }







    protected void Move(float speed, Transform transform, Transform player)
    {
        float move = speed * Time.deltaTime;
        // Find closest nodes
        if(AstarManger.instance.FindNearestNode(player.position) != EndNode)
        {
            currentPath.Clear();
        }

        if(currNode == null)
        {
            currNode = AstarManger.instance.FindNearestNode(transform.position);
        }

        if(currentPath.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentPath[x].transform.position.x, currentPath[x].transform.position.y, -2), speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, currentPath[x].transform.position) < 0.1f)
            {
                currNode = currentPath[x];
                currentPath.RemoveAt(x);
            }

        }
        else
        {
            if (currentPath.Count == 0)
            {
                currentPath = AstarManger.instance.GeneratePath(currNode,EndNode = AstarManger.instance.FindNearestNode(player.position));
                if (currentPath.Count > 1)
                {
                    currNode = currentPath[1];
                    currentPath.RemoveAt(0);
                }
            }
        }

        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    protected void MoveFromPlayer(float speed, Transform transform, Transform player)
    {
        float move = speed * Time.deltaTime;
        // Find closest nodes
        if (AstarManger.instance.FindFurthestNode(player.position, transform.position) != EndNode)
        {
            currentPath.Clear();
        }

        if (currNode == null)
        {
            currNode = AstarManger.instance.FindFurthestNode(transform.position, transform.position);
        }

        if (currentPath.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentPath[x].transform.position.x, currentPath[x].transform.position.y, -2), speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentPath[x].transform.position) < 0.1f)
            {
                currNode = currentPath[x];
                currentPath.RemoveAt(x);
            }

        }
        else
        {
            if (currentPath.Count == 0)
            {
                currentPath = AstarManger.instance.GeneratePath(currNode, EndNode = AstarManger.instance.FindFurthestNode(player.position, transform.position));
                if (currentPath.Count > 1 && currentPath != null)
                {
                    currNode = currentPath[1];
                    currentPath.RemoveAt(0);
                }
            }
        }

        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }



    public void GeneratePath(Transform transform, Transform player)
    {
        if (currentPath.Count == 0)
        {
            currentPath = AstarManger.instance.GeneratePath(AstarManger.instance.FindNearestNode(transform.position), AstarManger.instance.FindNearestNode(player.position));
        }
    }


    public override void Exit()
    {
        base.Exit();
        controller.onTakeDamage -= Hit;
    }

    public void Hit()
    {
        IsDamaged = controller.health.GetCurrentHealth() <= controller.health.GetMaxHealth() / 2;
        Debug.Log(IsDamaged);
        float chance;
        if (IsDamaged)
        {
             chance = 0.35f;
        }
        else {  chance = 0.25f; }
       
        if (Random.Range(0f, 1f) <= chance)
        {
            WasAttacked++;  
        }
        stateMachine.SwitchState(controller.Hit);
        
    }
}
