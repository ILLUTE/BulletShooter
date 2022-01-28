using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth { get; private set; }

    public Controller controller;

    private void Awake()
    {
        currentHealth = maxHealth;
        controller = this.GetComponent<Controller>();
    }
    public void CurrentHealthUpdate(float healthUpdate)
    {
        currentHealth += healthUpdate;
        controller.TookDamage();
    }
}
