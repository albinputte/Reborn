using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGhostState : BaseEnemyState<StoneGhostController>
{
    public static int WasAttacked;
    public static bool IsDamaged;
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

    protected void RaycastMovement(float speed, Transform transform, Transform player)
    {
        // Predefine directions for raycasting (avoiding recalculating every time)
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
        float minDistance = Mathf.Infinity;

        // Set a distance threshold for "too close"
        float tooCloseDistance = 3f; // Adjust this based on your game

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Flag to check if a valid direction was found
        bool foundValidDirection = false;

        if (distanceToPlayer > tooCloseDistance && !isRandomMoving)
        {
            // If the enemy is not too close to the player and not currently in random movement mode,
            // try to move toward the player
            foreach (var direction in directions)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f, controller.Layer);
                if (hit.collider == null) // No obstacle hit
                {
                    // Check if this direction brings the enemy closer to the player
                    float newDistanceToPlayer = Vector3.Distance(transform.position + direction * 5f, player.position);
                    if (newDistanceToPlayer < minDistance)
                    {
                        bestDirection = direction;
                        minDistance = newDistanceToPlayer;
                        foundValidDirection = true;
                    }
                }
            }

            // If a valid direction was found, move towards it
            if (foundValidDirection)
            {
                Vector3 targetPosition = transform.position + bestDirection * 5f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);
            }
            else
            {
                // Fallback to just moving towards the player if no valid direction is found
                Move(speed, transform, player);
            }
        }
        else if (distanceToPlayer <= tooCloseDistance && !isRandomMoving)
        {
            // If the enemy is too close, start moving randomly around the player
            isRandomMoving = true;

            // Choose a new random direction when random movement starts
            currentRandomDirection = directions[Random.Range(0, directions.Length)];
            randomMoveTimer = randomMoveDuration;  // Set the timer to the random movement duration
        }

        // Random movement behavior when isRandomMoving is true
        if (isRandomMoving)
        {
            if (randomMoveTimer <= 0f)
            {
                // Timer expired, stop random movement and allow the enemy to move towards the player again
                isRandomMoving = false;
                currentRandomDirection = Vector3.zero;  // Reset random direction
            }
            else
            {
                // Move in the current random direction
                Vector3 targetPosition = transform.position + currentRandomDirection * 5f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);

                // Decrease the timer by the delta time
                randomMoveTimer -= Time.deltaTime;
            }
        }

        // Flip the enemy based on the player's position
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
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, move);
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
