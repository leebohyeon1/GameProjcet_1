using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraController : MonoBehaviour, IListener
{
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private float ShakeIntensity = 3f;
    [SerializeField]
    private float ShakeTime = 0.1f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin perlin;

    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        StopShake();
        EventManager.Instance.AddListener(EVENT_TYPE.SHAKE_CAMERA, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        StartShake();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                StopShake();
            }
        }
    }

    void StartShake()
    {
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = ShakeIntensity;

        timer = ShakeTime;
    }

    void StopShake()
    {
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0;

        timer = 0;
    }
}
