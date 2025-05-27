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
    private bool HasInvinsiabilty;
    private bool IsInvinsiable;
    private float InvinciableTimer;
    private Vector2 tempKnockBack;
    private float DamageAmount;
    private float healAmount;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;
    public UnityEvent OnPassivRegen;
    [SerializeField] private bool IsPlayer;



    private void Awake()
    {
        InstantiateHealth(data);
      
    }

    public void InstantiateHealth(HealthData data)
    {
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        HasInvinsiabilty = data.hasInvincibilty;
        InvinciableTimer = data.invincibiltyTime;

        if (IsPlayer)
            InvokeHealthRegen();
    }



    public void TakeDamage(int amount)
    {
        if (!IsInvinsiable)
        {
            currentHealth -= amount;
          
            if (currentHealth <= 0)
            {
                IsInvinsiable = false;
                
                OnDeath?.Invoke();
              
            }
            else
            {
                if(HasInvinsiabilty)
                    StartCoroutine(StartInvinsiabiltyTimer(InvinciableTimer));
                OnTakeDamage?.Invoke();

            }

            
        }
     

    }
    public void StopCouritnes()
    {
        StopCoroutine(StartInvinsiabiltyTimer(InvinciableTimer));
      
    }

    public void heal(int amount, bool PassivRegen)
    {
        currentHealth += amount;
        healAmount = amount;
        if (currentHealth > maxHealth ) {
        currentHealth = maxHealth;
        }
        if(!PassivRegen)
            OnHeal?.Invoke();
        else
            OnPassivRegen?.Invoke();
    }
    public void InvokeHealthRegen()
    {
        StartCoroutine(HealthRegen());
    }
    public IEnumerator HealthRegen()
    {
        yield return new WaitForSeconds(2);
        if(StatSystem.instance.GetStat(StatsType.HealthRegen) > 0)
            heal((int)StatSystem.instance.GetStat(StatsType.HealthRegen), true);
        Invoke("InvokeHealthRegen",1);
    } 

    public void Hit(int Damage,Vector2 Knockback)
    {
        Debug.Log("Hit");
        tempKnockBack = Knockback;
        DamageAmount = Damage;
        TakeDamage(Damage);
    }
    public void TurnOffCollider(Collider2D other)
    {
        other.enabled = false;
    }

    public void TakeKnockBack(Rigidbody2D rb)
    {
        rb.AddForce(tempKnockBack, ForceMode2D.Impulse);
    }

    public void TakeKnockBack(Rigidbody2D rb, Vector2 Knockback)
    {
        rb.AddForce(Knockback, ForceMode2D.Impulse);
    }

    public void Flash(SpriteRenderer sprite)
    {
        StartCoroutine(EnemyFlash(sprite));
    }
    public void FlashPlayer(SpriteRenderer sprite)
    {
        StartCoroutine(PlayerFlash(sprite));
    }
    public IEnumerator PlayerFlash(SpriteRenderer sprite, float duration = 1f, float blinkInterval = 0.1f)
    {
        float elapsed = 0f;
        Color originalColor = sprite.color;

        while (elapsed < duration)
        {
            // Toggle alpha between 1 and 0 (visible and invisible)
            Color c = sprite.color;
            c.a = (c.a == 1f) ? 0f : 1f;
            sprite.color = c;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // Ensure sprite is fully visible at the end
        originalColor.a = 1f;
        sprite.color = originalColor;
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

    public void SpawnParticlesFromTarget(GameObject ParticlePrefab, Vector2 dir)
    {
        Vector3 direction = new Vector3(dir.x, dir.y).normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(ParticlePrefab, transform.position, rotation);
    }

    public void PlaySound(int index)
    {
        SoundType type = (SoundType)index;
        SoundManager.PlaySound(type);
    }
    public IEnumerator StartInvinsiabiltyTimer(float index)
    {
        IsInvinsiable = true;
        yield return new WaitForSeconds(index);
        IsInvinsiable = false;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void OnDamagePopUp(Transform transform)
    {
        DamagePopup.Create(transform.position, DamageAmount);
    }

    public void OnHealPopUp(Transform transform)
    {
        DamagePopup.CreateForPlayerHeal(transform.position, healAmount);
    }
    public void OnDamagePlayerPopUp(Transform transform)
    {
        DamagePopup.CreateForPlayer(transform.position, DamageAmount);
    }


}
