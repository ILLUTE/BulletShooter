using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager instance;

    public static WeaponManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponManager>();
            }

            return instance;
        }
    }

    public List<BaseWeaponAttack> weapons = new List<BaseWeaponAttack>();

    public BaseWeaponAttack currentWeapon;

    public static event Action OnWeaponSwitched;
    public static event Action OnWeaponShot;
    public static event Action<bool> OnWeaponReload;

    public bool IsWeaponReloading { get; private set; }
    public float ReloadTime { get; private set; }
    public float CurrentWeaponReloadTime
    {
        get
        {
            return currentWeapon.weapon.reloadingTime;
        }
    }

    public void AddWeapon(BaseWeaponAttack weapon)
    {
        if (!weapons.Contains(weapon))
        {
            weapons.Add(weapon);

            if (currentWeapon == null)
            {
                SwitchWeapon(0);
            }
            else
            {
                weapon.UnLoad();
            }
        }
    }

    public bool StartShootingCurrentWeapon(Transform target)
    {
        if(currentWeapon.Shoot(target))
        {
            OnWeaponShot?.Invoke();
            return true;
        }

        return false;
    }

    public void Flush()
    {
        weapons.Clear();
    }

    public void SwitchWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.UnLoad();
        }

        currentWeapon = weapons[index];

        OnWeaponSwitched?.Invoke();
        currentWeapon.Load();
        IsWeaponReloading = false;
    }

    public void ReloadCurrentWeapon()
    {
        if (currentWeapon.CanReload() && !IsWeaponReloading)
        {
            ReloadTime = currentWeapon.weapon.reloadingTime;
            IsWeaponReloading = true;
            OnWeaponReload?.Invoke(IsWeaponReloading);
        }
    }

    private void Update()
    {
        if (IsWeaponReloading)
        {
            if (ReloadTime > 0)
            {
                ReloadTime -= Time.unscaledDeltaTime;

                if (ReloadTime <= 0)
                {
                    OnWeaponReloaded();
                }
            }
        }
    }

    private void OnWeaponReloaded()
    {
        IsWeaponReloading = false;
        ReloadTime = 0.0f;
        currentWeapon.ReloadWeapon();
        OnWeaponReload?.Invoke(IsWeaponReloading);
    }
}
