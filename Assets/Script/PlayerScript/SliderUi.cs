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
        slider.maxValue = health.GetMaxHealth();
        slider.value = health.GetCurrentHealth();
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
