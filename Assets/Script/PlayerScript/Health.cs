using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagable
{
    public HealthData data;
    private int currentHealth;
    private int maxHealth;
    private Vector2 tempKnockBack;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;



    private void Awake()
    {
        InstantiateHealth(data);
    }

    public void InstantiateHealth(HealthData data)
    {
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
    }



    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 ) {
            OnDeath?.Invoke();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }

    }

    public void heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth ) {
        currentHealth = maxHealth;
        }
        OnHeal?.Invoke();
    }

    public void Hit(int Damage,Vector2 Knockback)
    {
        Debug.Log("Hit");
        tempKnockBack = Knockback;
        TakeDamage(Damage);
    }

    public void TakeKnockBack(Rigidbody2D rb)
    {
        rb.AddForce(tempKnockBack, ForceMode2D.Impulse);
    }

    public void Flash(SpriteRenderer sprite)
    {
        StartCoroutine(EnemyFlash(sprite));
    }   

    public IEnumerator EnemyFlash(SpriteRenderer sprite)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        StopCoroutine("EnemyFlash");
        yield return new WaitForSeconds(0.1f);
    }

    public void Die()
    {
        Destroy(gameObject, 0.35f);
    }

    public void SpawnParticles(GameObject ParticlePrefab)
    {
        Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
    }

    public void SpawnParticlesFromTarget(GameObject ParticlePrefab)
    {
        Vector3 direction = new Vector3(tempKnockBack.x, tempKnockBack.y).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(ParticlePrefab, transform.position, rotation);
    }

    public void PlaySound(int index)
    {
        SoundType type = (SoundType)index;
        SoundManager.PlaySound(type);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }


}
