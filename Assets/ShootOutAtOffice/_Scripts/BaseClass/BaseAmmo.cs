using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseAmmo : MonoBehaviour
{
    public abstract void OnShot(Transform spawn, Transform lookAtRef);
    public abstract void OnHit(HealthPoints hp);
}
