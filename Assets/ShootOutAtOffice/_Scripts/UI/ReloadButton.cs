using System;
using UnityEngine.UI;
using UnityEngine;

public class ReloadButton : MonoBehaviour
{
    public Image weaponIcon;
    public Image weaponFillIcon;
    private bool m_IsReloading;
    private void Awake()
    {
        WeaponManager.OnWeaponSwitched += WeaponSwitched;
        WeaponManager.OnWeaponReload += WeaponReloading;
    }

    private void WeaponReloading(bool reloading)
    {
        if(reloading)
        {
            weaponFillIcon.fillAmount = 0;
            m_IsReloading = true;
        }
        else
        {
            weaponFillIcon.fillAmount = 1;
            m_IsReloading = false;
        }
    }

    private void WeaponSwitched()
    {
        weaponFillIcon.fillAmount = 1;
    }

    public void ReloadWeapon()
    {
        WeaponManager.Instance.ReloadCurrentWeapon();
    }

    private void Update()
    {
        if(m_IsReloading)
        {
            weaponFillIcon.fillAmount = 1 - (WeaponManager.Instance.ReloadTime / WeaponManager.Instance.CurrentWeaponReloadTime);
        }
    }
}
