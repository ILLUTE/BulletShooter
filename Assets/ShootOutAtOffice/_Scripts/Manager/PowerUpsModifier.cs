using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsModifier : MonoBehaviour
{
    private static PowerUpsModifier instance;

    public static PowerUpsModifier Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PowerUpsModifier>();
            }

            return instance;
        }
    }

    private const float m_RateOfFireModifier_EnemiesKilledDuration = 5f;

    private float m_AppliedRateOfFireModifierOnEnemiesKilled;

    private bool m_IsRateOfFireModifierApplied;

    public float m_RateOfFireModifier_EnemiesKilled;

    public float TotalRateOfModifier
    {
        get
        {
            return m_RateOfFireModifier_EnemiesKilled;
        }
    }

    private void Awake()
    {
        EnemyManager.OnEnemyRemoved += CheckForModification;
    }

    private void CheckForModification(int count)
    {
       
    }

    private void Update()
    {
        if(m_AppliedRateOfFireModifierOnEnemiesKilled > 0)
        {

        }
    }
}
