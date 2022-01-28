using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class StateController : Controller
{
    public Laser enemyAlarmSystem;
    public NavMeshAgent enemyAgent;
    public Transform enemyTransform;
    public Transform headTransform;
    public HealthPoints health;
    public Animator enemyAnim;
    public Collider m_Collider;
    public List<Transform> patrolPoints = new List<Transform>();
    public BaseState currentState;
    public BaseState remainState;
    public Image visionRange;
    public RectTransform rangeTransform;
    public BaseWeaponAttack weapon;
    private HealthUI healthUI;
    

    public Rig shootRig;
    public Rig idleRig;

    public int nextWayPointIndex = 0;
    public float agentWalkSpeed = 3f;
    public float agentRunSpeed = 6f;
    public float m_AimTime = .3f;
    public float m_IdleTime = 3.0f;
    public float m_AllowSuspiciousTime = 1.0f;


    private float m_InflatedRadiusTime = 2.0f;
    public bool IsRotating;
    public bool m_CanChase;


    private bool IsSetUp;
    private bool InflatedInSightRadius;
    private bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            isDead = value;
        }
    }

    public void Dead()
    {
        if (!IsDead)
        {
            IsDead = true;
            idleRig.weight = 0;
            healthUI.gameObject.SetActive(false);
            enemyAgent.isStopped = true;
            m_Collider.enabled = false;
            enemyAnim.applyRootMotion = true;
            enemyAnim.SetTrigger("Dead");
            visionRange.gameObject.SetActive(false);
            EnemyManager.Instance.OnEnemyKilled(this);
            DOVirtual.DelayedCall(2f, () =>
            {
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            });
        }
    }    

    public override void TookDamage()
    {
        healthUI.SetHealth(health.currentHealth, health.maxHealth);

        if (health.currentHealth <= 0)
        {
            Dead();
            return;
        }

        if(enemyAlarmSystem.m_TargetBody == null)
        {
            InflatedInSightRadius = true;
            enemyAlarmSystem.inSightRadius = 180;
            m_CanChase = true;
            UpdateSightRadiusUI();
        }
    }

    public void Shoot(Transform t)
    {
        weapon.Shoot(t);
    }

    public void SetPatrolPoints(HealthUI ui,Transform[] wavePoints, int length = 2)
    {
        healthUI = ui;
        patrolPoints.Clear();

        for (int i = 0; i < length; i++)
        {
            patrolPoints.Add(wavePoints[i]);
        }
        enemyAgent.updateRotation = false;
        IsSetUp = true;
        enemyAgent.enabled = true;
        healthUI.SetPosition(headTransform.position);
        healthUI.SetHealth(health.currentHealth, health.maxHealth);
        UpdateSightRadiusUI();
    }

    private void UpdateSightRadiusUI()
    {
        visionRange.DOFillAmount(enemyAlarmSystem.inSightRadius / 180, 0.5f);
        rangeTransform.localRotation = Quaternion.Euler(90, 180 - (enemyAlarmSystem.inSightRadius), rangeTransform.rotation.z);
        float size = enemyAlarmSystem.alarmTriggerRadius * 2;
        rangeTransform.sizeDelta = new Vector2(size, size);
    }

    private void Awake()
    {
        if (enemyTransform == null)
        {
            enemyTransform = this.GetComponent<Transform>();
        }

        if (enemyAlarmSystem == null)
        {
            enemyAlarmSystem = this.GetComponent<Laser>();
        }

        if (health == null)
        {
            health = this.GetComponent<HealthPoints>();
        }
        if (m_Collider == null)
        {
            m_Collider = this.GetComponent<Collider>();
        }

        enemyAgent.enabled = false;
    }

    public void ChangeState(BaseState state)
    {
        if (state != remainState)
        {
            currentState = state;
        }
    }

    private void Update()
    {
        if (!IsSetUp || IsDead)
        {
            return;
        }

        if (InflatedInSightRadius)
        {
            if (m_InflatedRadiusTime > 0)
            {
                m_InflatedRadiusTime -= Time.unscaledDeltaTime;

                if (m_InflatedRadiusTime <= 0)
                {
                    enemyAlarmSystem.inSightRadius = 60;
                    InflatedInSightRadius = false;
                    UpdateSightRadiusUI();
                }
            }
        }

        healthUI.SetPosition(headTransform.position);
        currentState.UpdateState(this);
        enemyAnim.SetFloat("Movement", enemyAgent.speed/agentRunSpeed, 0.1f, Time.deltaTime);
    }
}