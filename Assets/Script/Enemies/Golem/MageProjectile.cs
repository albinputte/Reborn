using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MageProjectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    public float speed = 6f;
    public int damage = 5;

    private Vector2 direction;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Required physics setup
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }


    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        // Apply velocity ONCE
        rb.velocity = direction * speed;

        // Auto-destroy after time
        Destroy(gameObject, 5f);
    }


  
}
