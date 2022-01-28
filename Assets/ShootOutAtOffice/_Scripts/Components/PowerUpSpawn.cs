using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    public float powerUpTime = 3f;
    public float weaponModifier = 3f;
    public WeaponModifier modifier = WeaponModifier.RateOfFire;

    public Laser m_PowerUpAlarm;

    private void Update()
    {
        if (m_PowerUpAlarm.m_TargetBody)
        {
            WeaponManager.Instance.currentWeapon.AddModifier(modifier, weaponModifier, powerUpTime);
            Destroy(this.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif
}
