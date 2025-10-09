using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUi : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider slider;

    private void Start()
    {
        SceneManger.instance.OnAllEssentialScenesLoaded += PrepareRefrences;
    }

    public void PrepareRefrences()
    {
        health = GameObject.Find("Player").gameObject.GetComponentInChildren<Health>();
        slider.maxValue = health.GetMaxHealth();
        slider.value = health.GetCurrentHealth();
        health.OnTakeDamage.AddListener(UpdateHealth);
        health.OnHeal.AddListener(UpdateHealth);
        health.OnPassivRegen.AddListener(UpdateHealth);
    }

    public void UpdateHealth()
    {
        if (health.GetCurrentHealth() > 0)
        {
            slider.value = health.GetCurrentHealth();
        }
        else { slider.value = 0; }
    }

    public void UpdateMaxHealth()
    {
        slider.maxValue = health.GetMaxHealth();
    }


}
