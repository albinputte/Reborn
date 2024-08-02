using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public HealthData data;
    private int currentHealth;
    private int maxHealth;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;


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




}
