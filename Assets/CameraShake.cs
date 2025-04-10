using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    public static CameraShake instance;
    private float _shakeTime;
    private float startingIntensity;
    private float totalShakeTime;
    public void Awake()
    {
        instance = this;
        _cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intesity, float shakeTime)
    {
        CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        basicMultiChannelPerlin.m_AmplitudeGain = intesity;
        startingIntensity = intesity;
        totalShakeTime = shakeTime;
        _shakeTime = shakeTime;
  
    }

    public void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            basicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1-( _shakeTime /totalShakeTime));
         

        }
    }

}
