using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rifle_M4A4 : BaseWeaponAttack
{
    [Header("References")]

    public Transform bulletSpawn;

    public ParticleSystem gunShot;

    public AnimatorOverrideController overrideController;

    public Transform playerLoadTransform;

    public Transform playerUnloadTransform;

    public TrailRenderer bulletTrail;

    public bool enemyWeapon;

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
        if (!enemyWeapon)
        {
            WeaponManager.Instance.AddWeapon(this);
        }
    }

    public override bool Shoot(Transform target)
    {
        bool playerShot = false;

        if (target != null)
        {
            Vector3 distance = (target.position - (m_WeaponTransform.position));
            m_WeaponTransform.LookAt(target);
            if (distance.magnitude <= weapon.weaponTriggerRadius)
            {
                if (Time.time > m_LastShotTime + (1 / (weapon.rateOfFire)))
                {
                    TrailRenderer bullet = Instantiate(bulletTrail);
                    bullet.AddPosition(bulletSpawn.position);
                    if (Physics.Raycast(bulletSpawn.position, transform.forward * distance.magnitude, out RaycastHit hit))
                    {
                        HealthPoints temp = hit.collider.GetComponent<HealthPoints>();
                        if (temp != null)
                        {
                            temp.CurrentHealthUpdate(-1 * weapon.hitDmg);
                        }

                        hitParticle.transform.position = hit.point;
                        hitParticle.transform.forward = hit.normal;
                        hitParticle.Play();
                        bullet.transform.position = hit.point;
                    }
                    playerShot = true;
                    m_LastShotTime = Time.time;
                    weapon.currentMagazineAmount--;
                    gunShot.Play();
                    CameraManager.Instance.Shake(2, 0.15f);

                    if (weapon.currentMagazineAmount == 0)
                    {
                        if (!enemyWeapon)
                        {
                            WeaponManager.Instance.ReloadCurrentWeapon();
                        }
                        else
                        {
                            ReloadWeapon();
                        }
                    }
                }
            }
        }

        return playerShot;
    }

    public override void Load()
    {
        this.gameObject.SetActive(true); ;
    }

    public override void UnLoad()
    {
        this.gameObject.SetActive(false);
    }

    public bool IsReloading()
    {
        return false;
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

        if (duration > 0)
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
