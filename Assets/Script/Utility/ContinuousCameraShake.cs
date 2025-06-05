using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ContinuousCameraShake : MonoBehaviour
{
    [Header("Virtual Cameras to Shake")]
    public List<CinemachineVirtualCamera> virtualCameras;

    [Header("Shake Settings")]
    public float shakeAmplitude = 2f;
    public float shakeFrequency = 2f;
    public float fadeDuration = 0.5f;

    private Dictionary<CinemachineVirtualCamera, Coroutine> fadeCoroutines = new();

    void OnEnable()
    {
        SetShake(true);
    }

    void OnDisable()
    {
        //SetShake(false);
    }

    void SetShake(bool enable)
    {
        foreach (var vcam in virtualCameras)
        {
            if (vcam == null) continue;

            if (fadeCoroutines.ContainsKey(vcam) && fadeCoroutines[vcam] != null)
                StopCoroutine(fadeCoroutines[vcam]);

            fadeCoroutines[vcam] = StartCoroutine(FadeShake(vcam, enable));
        }
    }

    IEnumerator FadeShake(CinemachineVirtualCamera vcam, bool enable)
    {
        var noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null) yield break;

        float startAmp = noise.m_AmplitudeGain;
        float targetAmp = enable ? shakeAmplitude : 0f;

        float startFreq = noise.m_FrequencyGain;
        float targetFreq = enable ? shakeFrequency : 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            noise.m_AmplitudeGain = Mathf.Lerp(startAmp, targetAmp, t);
            noise.m_FrequencyGain = Mathf.Lerp(startFreq, targetFreq, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = targetAmp;
        noise.m_FrequencyGain = targetFreq;
    }
}
