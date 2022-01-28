using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    public static ResourceManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ResourceManager>();
            }

            return instance;
        }
    }

    public Player r_Player;
    public StateController r_Enemy;
    public PowerUpSpawn r_PowerUpRateModifier_1;
    public WeaponSpawn r_WeaponSpawn_1;
}
