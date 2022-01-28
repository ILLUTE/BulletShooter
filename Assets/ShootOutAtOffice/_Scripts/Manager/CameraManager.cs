using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;

    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraManager>();
            }

            return instance;
        }
    }

    public CinemachineVirtualCamera m_Camera;
    private CinemachineBasicMultiChannelPerlin m_ShakeModifier;
    public float m_ShakeTime;
    public LayerMask obstacle;
    MeshRenderer[] wall;
    int length = 0;
    private void Awake()
    {
        m_ShakeModifier = m_Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        m_ShakeModifier.m_AmplitudeGain = 0;
    }
    public void SetPlayer(Transform player)
    {
        m_Camera.m_Follow = player;
    }

    public void Shake(float intensity, float durationOfShake)
    {
        m_ShakeModifier.m_AmplitudeGain = intensity;
        m_ShakeTime = durationOfShake;
    }

    private void Update()
    {
        if (m_ShakeTime > 0)
        {
            m_ShakeTime -= Time.deltaTime;

            if (m_ShakeTime <= 0)
            {
                m_ShakeModifier.m_AmplitudeGain = 0;
            }
        }

        Vector3 direction = m_Camera.m_Follow.position - this.transform.position;
        float distance = direction.magnitude;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance, obstacle))
        {
            wall = hit.collider.GetComponentsInChildren<MeshRenderer>();
            length = wall.Length;
            for (int i = 0; i < length; i++)
            {
                wall[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }


        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                wall[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            length = 0;
        }
    }
}
