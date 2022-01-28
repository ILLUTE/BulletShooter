using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pistol : BaseWeaponAttack
{
    [Header("References")]
   
    public Transform bulletSpawn;

    public ParticleSystem gunShot;

    private Transform m_WeaponTransform;

    private float m_LastShotTime;

    private void Awake()
    {
        if (m_WeaponTransform == null)
        {
            m_WeaponTransform = this.GetComponent<Transform>();
        }
    }

    private void OnEnable()
    {
        WeaponManager.Instance.AddWeapon(this);
    }

    public override bool Shoot(Transform target)
    {
        bool triggered = false;

        if (target != null)
        {
            Vector3 distance = (target.position - (m_WeaponTransform.position));

            if (distance.magnitude <= weapon.weaponTriggerRadius)
            {
                if (Time.time > m_LastShotTime + (1 / (weapon.rateOfFire)))
                {
                    if (Physics.Raycast(m_WeaponTransform.position, distance, out RaycastHit hit))
                    {
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            hit.collider.GetComponent<HealthPoints>().CurrentHealthUpdate((-weapon.hitDmg));
                        }
                    }

                    triggered = true;

                    m_LastShotTime = Time.time;
                    gunShot.Play();
                    CameraManager.Instance.Shake(2, 0.15f);
                }
            }
        }
        

        return triggered;
    }

    public override void Load()
    {
        this.gameObject.SetActive(true); ;
    }

    public override void UnLoad()
    {
        this.gameObject.SetActive(false);
    }

    public override void AddModifier(WeaponModifier modifier, float weaponModifier, float duration)
    {
        switch (modifier)
        {
            case WeaponModifier.Radius:
                weapon.weaponTriggerRadius += weaponModifier;
                break;
            case WeaponModifier.RateOfFire:
                weapon.rateOfFire += weaponModifier;
                break;
        }

        if (duration != -1)
        {
            DOVirtual.DelayedCall(duration, () =>
            {
                switch (modifier)
                {
                    case WeaponModifier.Radius:
                        weapon.weaponTriggerRadius -= weaponModifier;
                        break;
                    case WeaponModifier.RateOfFire:
                        weapon.rateOfFire -= weaponModifier;
                        break;
                }
            });
        }
    }
}