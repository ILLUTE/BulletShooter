using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HealthUI : MonoBehaviour
{
    public Transform m_Transform;
    public Image m_HealthBar;

    public Gradient gradient;

    private void Awake()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    public void SetPosition(Vector3 pos)
    {
        m_Transform.position = pos;
    }

    public void SetHealth(float currentHealth,float maxHealth)
    {
        m_HealthBar.DOKill(false);
        m_HealthBar.color = gradient.Evaluate(currentHealth / maxHealth);
        m_HealthBar.DOFillAmount(currentHealth / maxHealth, 0.1f);
    }
}
