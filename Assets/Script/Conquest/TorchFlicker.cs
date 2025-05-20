using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
[RequireComponent(typeof(Light2D))]
public class TorchFlicker : MonoBehaviour
{
    private Light2D lightToFlicker;

    [Header("Flicker Settings")]
    public bool flickerEnabled = true;
    [Range(0.3f, 3f)] public float minIntensity = 0.5f;
    [Range(0.3f, 3f)] public float maxIntensity = 1.2f;
    [Min(0f)] public float minTimeBetweenIntensity = 0.05f;
    [Min(0f)] public float maxTimeBetweenIntensity = 0.2f;
    [Min(0f)] public float minChangeAmount = 0.1f;

    private float currentTimer;
    private float nextFlickerDelay;
    private float lastUpdateTime;
    private float lastIntensity;

    private void OnEnable()
    {
        lightToFlicker = GetComponent<Light2D>();
        ValidateIntensityBounds();
        lastUpdateTime = Time.realtimeSinceStartup;
        lastIntensity = minIntensity;
        SetNextFlickerDelay();
    }

    private void Update()
    {
        if (lightToFlicker == null || !flickerEnabled)
        {
            if (lightToFlicker != null)
                lightToFlicker.intensity = minIntensity;
            return;
        }

#if UNITY_EDITOR
        float deltaTime = Application.isPlaying ? Time.deltaTime : (Time.realtimeSinceStartup - lastUpdateTime);
#else
        float deltaTime = Time.deltaTime;
#endif

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

            lightToFlicker.intensity = newIntensity;
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