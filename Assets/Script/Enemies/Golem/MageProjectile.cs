using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 5;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health))
        {
            //health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
