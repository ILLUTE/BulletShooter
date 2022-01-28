using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
using System;

public class Player : Controller
{
    public float playerSpeed;
    public float turnSmoothTime = 0.1f;
    public float turnOffsetAngle = 5;
    public float idleTime = 0.3f;

    public bool IsDead { get; private set; }

    private Vector3 movement = Vector3.zero;
    private Vector3 enemyDirection = Vector3.zero;
    private float horizontal;
    private float forward;
    private float brackeys_Velocity;
    private float lastMoveTime;

    private float m_AimTime = 0.15f;

    private bool canStance;
    private bool IsShooting;

    public Animator player_anim;
    public Transform m_Transform;
    public CharacterController m_Controller;
    public Laser playerTrigger;
    public Rig shootLayer;
    public Transform playerUISpawn;
    public HealthPoints m_HealthPoints;

    private HealthUI m_FollowUI;

    public TwoBoneIKConstraint m_LeftHandIK;
    public TwoBoneIKConstraint m_RightHandIK;

    public Transform leftHandPos;
    public Transform rightHandPos;

    private void Awake()
    {
        playerTrigger = this.GetComponent<Laser>();
        m_Transform = this.GetComponent<Transform>();
        m_Controller = this.GetComponent<CharacterController>();
        m_HealthPoints = this.GetComponent<HealthPoints>();

        WeaponManager.OnWeaponReload += WeaponReloadAnimation;
        WeaponManager.OnWeaponSwitched += WeaponSwitched;

    }

    private void WeaponSwitched()
    {

    }

    private void WeaponReloadAnimation(bool reloading)
    {
       if(reloading)
        {
            // Play ReloadAnimation
        }
    }

    private void Start()
    {
        InputSystem.Instance.OnButtonPressed += ButtonPressed;
        
    }

    public override void TookDamage()
    {
        m_FollowUI.SetHealth(m_HealthPoints.currentHealth, m_HealthPoints.maxHealth);

        if(m_HealthPoints.currentHealth<=0)
        {
            Kill();
        }
    }

    public void SetPlayer(HealthUI ui)
    {
        m_FollowUI = ui;
        m_FollowUI.SetPosition(playerUISpawn.position);
        m_FollowUI.SetHealth(m_HealthPoints.currentHealth, m_HealthPoints.maxHealth);
        shootLayer.weight = 0;
    }

    private void ButtonPressed(Vector2 vec)
    {
        horizontal = vec.x;
        forward = vec.y;
    }

    public void Kill()
    {
        if (!IsDead)
        {
            IsDead = true;
            m_Controller.enabled = false;
            player_anim.applyRootMotion = true;
            player_anim.SetTrigger("Dead");
            DOVirtual.DelayedCall(2f, () =>
            {
                GameManager.Instance.RaisePlayerKilled();
            });
        }
    }

    void Update()
    {
        if (!IsDead)
        {
            movement = new Vector3(horizontal, 0, forward);

            if (movement.magnitude >= 0.1f)
            {
                shootLayer.weight = 0;

                float turnAngle = Mathf.Atan2(movement.normalized.x, movement.normalized.z) * Mathf.Rad2Deg;

                float angle = Mathf.SmoothDampAngle(m_Transform.eulerAngles.y, turnAngle, ref brackeys_Velocity, turnSmoothTime);

                m_Transform.rotation = Quaternion.Euler(0, angle, 0);

                m_Controller.Move(playerSpeed * Time.deltaTime * movement.normalized);

                lastMoveTime = Time.time;

                m_FollowUI.SetPosition(playerUISpawn.position);
            }
            else
            {
                if (Time.time > lastMoveTime + idleTime)
                {
                    if (playerTrigger.m_TargetBody != null)
                    {
                        canStance = true;

                        enemyDirection = ((playerTrigger.m_TargetBody.transform.position) - (m_Transform.position));

                        float turnAngle = Mathf.Atan2(enemyDirection.normalized.x, enemyDirection.normalized.z) * Mathf.Rad2Deg;

                        float angle = Mathf.SmoothDampAngle(m_Transform.eulerAngles.y, turnAngle, ref brackeys_Velocity, turnSmoothTime);

                        m_Transform.rotation = Quaternion.Euler(0, angle, 0);

                        shootLayer.weight += Time.deltaTime * (1 / m_AimTime);

                        if (brackeys_Velocity <= .5f)
                        {
                            if (shootLayer.weight >= 1)
                            {
                                if (!WeaponManager.Instance.IsWeaponReloading)
                                {
                                    if (WeaponManager.Instance.StartShootingCurrentWeapon(playerTrigger.m_TargetBody.transform))
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        shootLayer.weight -= (1 / m_AimTime) * Time.deltaTime;
                    }
                }
            }

            player_anim.SetFloat("Movement", movement.magnitude);
            player_anim.SetBool("ShootingStance", canStance);
            player_anim.SetBool("Shooting", IsShooting);
        }
    }
}
