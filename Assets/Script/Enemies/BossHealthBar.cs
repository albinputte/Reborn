using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour

{
    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject root; // parent object to enable/disable UI

    private Health bossHealth;
    public static BossHealthBar instance;


    private void Awake()
    {
        instance = this;
        if (root != null)
            root.SetActive(false);
    }


    public void SetBossHealthBar(Health health)
    {
        if (health == null)
            return;

        RemoveListeners();

        bossHealth = health;

        slider.maxValue = bossHealth.GetMaxHealth();
        slider.value = bossHealth.GetCurrentHealth();

        bossHealth.OnTakeDamage.AddListener(UpdateHealth);
        bossHealth.OnHeal.AddListener(UpdateHealth);
        bossHealth.OnPassivRegen.AddListener(UpdateHealth);
        bossHealth.OnDeath.AddListener(OnBossDeath);

        Activate();
    }

   
    public void Activate()
    {
        if (root != null)
            root.SetActive(true);
    }

    public void Deactivate()
    {
        if (root != null)
            root.SetActive(false);

        RemoveListeners();
        bossHealth = null;
    }


    public void UpdateHealth()
    {
        if (bossHealth == null)
            return;

        slider.value = Mathf.Max(0, bossHealth.GetCurrentHealth());
    }

    public void UpdateMaxHealth()
    {
        if (bossHealth == null)
            return;

        slider.maxValue = bossHealth.GetMaxHealth();
    }


    private void OnBossDeath()
    {
        Deactivate();
    }


    private void RemoveListeners()
    {
        if (bossHealth == null)
            return;

        bossHealth.OnTakeDamage.RemoveListener(UpdateHealth);
        bossHealth.OnHeal.RemoveListener(UpdateHealth);
        bossHealth.OnPassivRegen.RemoveListener(UpdateHealth);
        bossHealth.OnDeath.RemoveListener(OnBossDeath);
    }
}

