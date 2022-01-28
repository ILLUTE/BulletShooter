using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseWeaponAttack : MonoBehaviour
{
    public Weapons weapon;
    public ParticleSystem hitParticle;
    public abstract bool Shoot(Transform target);
    public abstract void Load();
    public abstract void UnLoad();
    public abstract void AddModifier(WeaponModifier modifier, float weaponModifier, float duration);
    public void ReloadWeapon()
    {
        weapon.currentMagazineAmount = weapon.magazineCapacity;
    }
    public bool CanReload()
    {
        return weapon.currentMagazineAmount < weapon.magazineCapacity;
    }
}

public enum WeaponModifier
{
    RateOfFire,
    Radius
}
