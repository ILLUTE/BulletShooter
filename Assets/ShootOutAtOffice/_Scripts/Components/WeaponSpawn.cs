using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public Laser m_Pickup;

    void Update()
    {
        if (m_Pickup.m_TargetBody)
        {
            WeaponManager.Instance.SwitchWeapon(1);
            Destroy(this.gameObject);
        }
    }
}
