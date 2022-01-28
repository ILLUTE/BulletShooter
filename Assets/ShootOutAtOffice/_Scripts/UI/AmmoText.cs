using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class AmmoText : MonoBehaviour
{
    public TextMeshProUGUI m_Ammo;
    private StringBuilder m_ShowText;

    private void Awake()
    {
        WeaponManager.OnWeaponReload += WeaponReloaded;
        WeaponManager.OnWeaponSwitched += WeaponSwitch;
        WeaponManager.OnWeaponShot += WeaponSwitch;
    }

    private void WeaponSwitch()
    {
        CheckStringBuilder();
        m_ShowText.Length = 0;
        m_ShowText.Append(string.Format("{0}/{1}", WeaponManager.Instance.currentWeapon.weapon.currentMagazineAmount, WeaponManager.Instance.currentWeapon.weapon.magazineCapacity));
        m_Ammo.text = m_ShowText.ToString();
    }

    private void CheckStringBuilder()
    {
        if (m_ShowText == null)
        {
            m_ShowText = new StringBuilder();
        }
    }

    private void WeaponReloaded(bool reloading)
    {
        CheckStringBuilder();
        m_ShowText.Length = 0;
        m_ShowText.Append(reloading ? "Reloading..." : string.Format("{0}/{1}", WeaponManager.Instance.currentWeapon.weapon.currentMagazineAmount, WeaponManager.Instance.currentWeapon.weapon.magazineCapacity));
        m_Ammo.text = m_ShowText.ToString();
    }
}
