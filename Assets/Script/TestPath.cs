using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TopDownPathfinding : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public float stopDistance = 1f;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float directionSmoothTime = 0.3f;
    public float rotationSpeed = 5f;

    [Header("Raycast Settings")]
    public float rayDistance = 1.5f;
    public LayerMask obstacleLayer;
    public float rayOriginBuffer = 0.2f;
    public int rayCount = 8;
    public float checkInterval = 0.2f;
    public float rayArc = 180f; // Degrees of arc for raycasting

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;

    // Private variables
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 chosenDirection;
    private float checkTimer;
    private Vector2 directionSmoothVelocity;
    private Quaternion[] angleRotations;
    private Vector2 currentFacingDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        InitializeRayDirections();
    }

    void InitializeRayDirections()
    {
        // Initialize rays based on the given ray arc and count
        angleRotations = new Quaternion[rayCount];
        float angleStep = rayArc / (rayCount - 1);
        float startAngle = -rayArc / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            angleRotations[i] = Quaternion.Euler(0, 0, startAngle + angleStep * i);
        }
    }

    void Update()
    {
        if (target == null) return;

        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            UpdateDirection();
            checkTimer = checkInterval;
        }

        UpdateSpriteDirection();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        float distToTarget = Vector2.Distance(transform.position, target.position);

        if (distToTarget <= stopDistance)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            Vector2 smoothDirection = Vector2.SmoothDamp(
                rb.velocity.normalized,
                chosenDirection.normalized,
                ref directionSmoothVelocity,
                directionSmoothTime);

            rb.velocity = smoothDirection * moveSpeed;

            // Update facing direction based on actual movement
            if (rb.velocity.magnitude > 0.1f)
            {
                currentFacingDirection = rb.velocity.normalized;
            }
        }
    }

    void UpdateDirection()
    {
        Vector2 toTarget = (target.position - transform.position).normalized;

        float bestScore = float.MinValue;
        Vector2 bestDirection = toTarget;

        for (int i = 0; i < rayCount; i++)
        {
            // Rotate ray direction based on current facing direction
            Vector2 rayLocalDir = angleRotations[i] * Vector2.right;
            Vector2 rayWorldDir = RotateVector(rayLocalDir, currentFacingDirection);

            // Get the ray's origin, slightly offset to avoid starting inside the character.
            Vector2 rayOrigin = GetRayOrigin(rayWorldDir);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayWorldDir, rayDistance, obstacleLayer);

            // If no hit, calculate the best direction based on alignment with the target
            if (!hit)
            {
                // Calculate a score based on how aligned the ray is with the target.
                float alignmentScore = Vector2.Dot(rayWorldDir.normalized, toTarget);

                // Calculate the side obstacle avoidance score.
                float distanceScore = GetSideObstacleScore(rayWorldDir);

                // Combine the scores to get a final score for this direction
                float totalScore = alignmentScore * 0.9f + distanceScore * 0.1f;

                // If this direction is better, update the best direction
                if (totalScore > bestScore)
                {
                    bestScore = totalScore;
                    bestDirection = rayWorldDir.normalized;
                }
            }
        }

        // Update chosen direction
        chosenDirection = bestDirection;

        // Debugging: Log the chosen direction to help visualize
        Debug.Log("Chosen Direction: " + chosenDirection);
    }

    // This function rotates a vector by the given facing direction
    Vector2 RotateVector(Vector2 vector, Vector2 facingDirection)
    {
        // Compute the angle from the facing direction
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x);

        // Create a rotation matrix
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        // Rotate the vector by multiplying with the rotation matrix
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    // Side obstacle avoidance score
    float GetSideObstacleScore(Vector2 dir)
    {
        float score = 0f;
        int sideRays = 1;
        float sideAngle = 30f;

        for (int i = -sideRays; i <= sideRays; i++)
        {
            if (i == 0) continue;

            Vector2 sideDir = RotateVector(dir, Quaternion.Euler(0, 0, sideAngle * i) * Vector2.right);
            RaycastHit2D hit = Physics2D.Raycast(GetRayOrigin(sideDir), sideDir, rayDistance * 0.5f, obstacleLayer);

            score += hit ? hit.distance / (rayDistance * 0.5f) : 1f;
        }

        return score / (sideRays * 2);
    }

    private Vector2 GetRayOrigin(Vector2 dir)
    {
        if (col == null) return transform.position;

        Bounds bounds = col.bounds;
        Vector2 origin = transform.position;
        origin.x += Mathf.Sign(dir.x) * (bounds.extents.x - rayOriginBuffer);
        origin.y += Mathf.Sign(dir.y) * (bounds.extents.y - rayOriginBuffer);
        return origin;
    }

    void UpdateSpriteDirection()
    {
        if (spriteRenderer == null) return;

        // Only update the rotation if there is actual movement
        if (rb.velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(currentFacingDirection.y, currentFacingDirection.x) * Mathf.Rad2Deg;
            spriteRenderer.transform.rotation = Quaternion.Lerp(
                spriteRenderer.transform.rotation,
                Quaternion.Euler(0, 0, angle),
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Optionally, you can keep the sprite facing the last direction
            spriteRenderer.transform.rotation = Quaternion.identity;
        }
    }

    void OnDrawGizmos()
    {
        if (target == null || !Application.isPlaying || angleRotations == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, target.position);

        Vector2 toTarget = (target.position - transform.position).normalized;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayLocalDir = angleRotations[i] * Vector2.right;
            Vector2 rayWorldDir = RotateVector(rayLocalDir, currentFacingDirection);
            Vector2 origin = GetRayOrigin(rayWorldDir);

            RaycastHit2D hit = Physics2D.Raycast(origin, rayWorldDir, rayDistance, obstacleLayer);
            Gizmos.color = !hit ? Color.Lerp(Color.yellow, Color.green, Vector2.Dot(rayWorldDir.normalized, toTarget)) : Color.red;
            Gizmos.DrawRay(origin, rayWorldDir.normalized * rayDistance);
        }

        Gizmos.color = Color.cyan;
        Vector2 chosenOrigin = GetRayOrigin(chosenDirection);
        Gizmos.DrawRay(chosenOrigin, chosenDirection.normalized * rayDistance);
    }
}
