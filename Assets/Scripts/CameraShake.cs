using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cum;
    private float ShakeTimer;
    private float ShakeTimerTotal;
    private float startingIntensity; 
    private void Awake()
    {
        Instance = this;
        cum = GetComponent<CinemachineVirtualCamera>();
    }
    public void ShakeCamera(float intensity,float time)
    {
        CinemachineBasicMultiChannelPerlin BasicPerlin = 
            cum.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        BasicPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        ShakeTimer = time;
        ShakeTimerTotal = time;
    }
    private void Update()
    {
        if (ShakeTimer>0)
        {
            ShakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin BasicPerlin =
                cum.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            BasicPerlin.m_AmplitudeGain=
            Mathf.Lerp(startingIntensity,0,1-(ShakeTimer/ShakeTimerTotal));
        
        }
    }
}
