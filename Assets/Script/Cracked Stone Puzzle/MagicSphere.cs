using System.Collections;
using UnityEngine;
using System;
public class MagicSphere : MonoBehaviour
{
    [HideInInspector] public PressurePlateMagicSpawner originPlate;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        if (originPlate != null)
        {
            originPlate.OnMagicSphereDestroyed();
        }
    }


    // function for health when taking damage making object fly in opposit direction of player and set animation trigger "Hit"
    public void TakeDamage()
    {
        animator.SetTrigger("Hit");

        Vector2 direction = Vector2.zero;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            direction = (Vector2)transform.position - (Vector2)player.transform.position;
            direction.Normalize();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * 5f, ForceMode2D.Impulse);
            }
        }
        // Start the coroutine to destroy the object after a delay
        StartCoroutine(Destroyself());
    }

    private IEnumerator Destroyself()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    //when colliding with anything destroy self
    //when colliding with enemy destroy self and deal dmg to nemy (tiny amount)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            Health enemy = collision.GetComponent<Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(10);
                Destroy(gameObject);

            }
        }


        if (collision.CompareTag("CrackedStone"))
        {
            collision.GetComponent<CrackedStone>().HitBySphere();
            Destroy(gameObject);

        }
        if (collision.CompareTag("Cliff"))
        {
            Destroy(gameObject);
        }
       

    }
}
