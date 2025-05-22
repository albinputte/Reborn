using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
[RequireComponent(typeof(Light2D))]
public class TorchFlicker : MonoBehaviour
{
    private Light2D lightToFlicker;

    [Header("Flicker Settings")]
    [Range(0.3f, 3f)] public float minIntensity = 0.5f;
    [Range(0.3f, 3f)] public float maxIntensity = 1.2f;
    [Min(0f)] public float minTimeBetweenIntensity = 0.05f;
    [Min(0f)] public float maxTimeBetweenIntensity = 0.2f;
    [Min(0f)] public float minChangeAmount = 0.1f;
    [Header("Radius Flicker Settings")]
    public bool flickerRadius = true;
    [Range(0.1f, 2f)] public float radiusMultiplier = 0.1f; // amount of radius change based on intensity

    private float currentTimer;
    private float nextFlickerDelay;
    private float lastUpdateTime;
    private float lastIntensity;
    private float baseOuterRadius;
    private float baseInnerRadius;

    private void OnEnable()
    {
        lightToFlicker = GetComponent<Light2D>();
        ValidateIntensityBounds();

        lastUpdateTime = Time.realtimeSinceStartup;
        lastIntensity = minIntensity;

        // Store base radius
        baseOuterRadius = lightToFlicker.pointLightOuterRadius;
        baseInnerRadius = lightToFlicker.pointLightInnerRadius;

        SetNextFlickerDelay();
    }

    private void Update()
    {
        if (lightToFlicker == null)
            return;

        if (!Application.isPlaying)
        {
            lightToFlicker.intensity = minIntensity;
            if (flickerRadius)
            {
                lightToFlicker.pointLightOuterRadius = baseOuterRadius;
                lightToFlicker.pointLightInnerRadius = baseInnerRadius;
            }
            return;
        }

        float deltaTime = Time.deltaTime;
        currentTimer += deltaTime;

        if (currentTimer >= nextFlickerDelay)
        {
            float newIntensity;
            int safetyCounter = 0;

            do
            {
                newIntensity = Random.Range(minIntensity, maxIntensity);
                safetyCounter++;
            }
            while (Mathf.Abs(newIntensity - lastIntensity) < minChangeAmount && safetyCounter < 10);

            // Set intensity
            lightToFlicker.intensity = newIntensity;

            // Optionally set radius
            if (flickerRadius)
            {
                float flickerOffset = (newIntensity - minIntensity) * radiusMultiplier;
                lightToFlicker.pointLightOuterRadius = baseOuterRadius + flickerOffset;
                lightToFlicker.pointLightInnerRadius = baseInnerRadius + flickerOffset * 0.6f;
            }

            lastIntensity = newIntensity;
            currentTimer = 0f;
            SetNextFlickerDelay();
        }
    }

    private void SetNextFlickerDelay()
    {
        nextFlickerDelay = Random.Range(minTimeBetweenIntensity, maxTimeBetweenIntensity);
    }

    private void ValidateIntensityBounds()
    {
        if (minIntensity > maxIntensity)
        {
            Debug.LogWarning("Min Intensity is greater than Max Intensity. Swapping values.");
            (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
        }
        if (minTimeBetweenIntensity > maxTimeBetweenIntensity)
        {
            Debug.LogWarning("Min Flicker Time is greater than Max Flicker Time. Swapping values.");
            (minTimeBetweenIntensity, maxTimeBetweenIntensity) = (maxTimeBetweenIntensity, minTimeBetweenIntensity);
        }
    }
}