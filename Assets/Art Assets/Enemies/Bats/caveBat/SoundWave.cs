
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SoundWave : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 4f;
    public int maxBounces = 5;
    public LayerMask wallLayer;

    [Header("Bounce Polish")]
    public float bounceCooldown = 0.02f;   // 20 ms
    public float surfaceOffset = 0.01f;    // push off wall

    private Rigidbody2D rb;
    private Vector2 direction;
    private float remainingLife;
    private int bounceCount;
    private float lastBounceTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.isKinematic = true;
    }

    private void Start()
    {
        remainingLife = lifetime;
        direction = transform.up.normalized;
    }

    private void Update()
    {
        float moveDist = speed * Time.deltaTime;

        // 🔒 Prevent ultra-fast double bounces
        bool canBounce = Time.time - lastBounceTime > bounceCooldown;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            moveDist,
            wallLayer
        );

        if (hit.collider != null && canBounce)
        {
            lastBounceTime = Time.time;

            // Move exactly to hit point
            rb.position = hit.point + hit.normal * surfaceOffset;

            // Reflect direction
            direction = Vector2.Reflect(direction, hit.normal).normalized;

            // Rotate to face movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // Consume remaining distance AFTER hit
            float remainingMove =
                moveDist - hit.distance;

            rb.position += direction * remainingMove;

            // Lifetime reduction
            remainingLife *= 0.5f;
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            // Normal movement
            rb.position += direction * moveDist;
        }

        remainingLife -= Time.deltaTime;
        if (remainingLife <= 0f)
            Destroy(gameObject);
    }
}

