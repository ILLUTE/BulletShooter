using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    [Header("References")]
    public SpawnPoint m_PlayerSpawn;

    public SpawnPoint powerUpSpawn;

    public SpawnPoint weaponSpawn;

    public HealthUI[] healthPoints;

    private Player m_Player;

    private PowerUpSpawn powerUp;

    private WeaponSpawn weapon;

    [Header("Variables")]
    public bool useRaycastForBullets;

    public bool spawnPlayer;

    private bool m_EnemiesKilled;

    private bool IsGameOver = false;

    public static event Action<string> OnGameOver;

    private void Awake()
    {
        EnemyManager.OnEnemiesCleared += EnemiesCleared;
        OnGameOver += OnGameOverMethod;
    }

    private void OnGameOverMethod(string obj)
    {
        InputSystem.Instance.IsInputEnabled = false;
        WeaponManager.Instance.Flush();
        WeaponManager.Instance.currentWeapon = null;
    }

    public HealthUI GetHealthForController()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (!healthPoints[i].gameObject.activeSelf)
            {
                healthPoints[i].gameObject.SetActive(true);
                return healthPoints[i];

            }
        }

        return null;
    }

    private void Start()
    {
        ResetLevel();
    }

    private void EnemiesCleared()
    {
        m_EnemiesKilled = true;
    }

    public void RaisePlayerKilled()
    {
        IsGameOver = true;
        OnGameOver?.Invoke("You Lost.");
    }

    public void RaiseDoorReached()
    {
        if (m_EnemiesKilled && !IsGameOver)
        {
            IsGameOver = true;

            OnGameOver("You Won");
        }
    }

    public void ResetLevel()
    {
        for(int i =0;i<healthPoints.Length;i++)
        {
            healthPoints[i].gameObject.SetActive(false);
        }
        if (m_Player)
        {
            WeaponManager.Instance.Flush();
        }

        EnemyManager.Instance.DestroyEnemies(() =>
        {
            if (m_Player != null)
            {
                Destroy(m_Player.gameObject);
            }

            LoadLevel();
        });
    }

    public void LoadLevel()
    {
        if (powerUp == null)
        {
            powerUp = Instantiate(ResourceManager.Instance.r_PowerUpRateModifier_1); // No Properties with PowerUp.. Recycling therefore.
        }

        if (weapon == null)
        {
       //     weapon = Instantiate(ResourceManager.Instance.r_WeaponSpawn_1);
        }

       // weapon.transform.position = weaponSpawn.m_Spawn.position;

        powerUp.transform.position = powerUpSpawn.m_Spawn.position;

        if (spawnPlayer)
        {
            m_Player = Instantiate(ResourceManager.Instance.r_Player); // Reloading Player
        }
        else
        {
            m_Player = FindObjectOfType<Player>();
        }

        m_Player.m_Transform.position = m_PlayerSpawn.m_Spawn.position;

        m_Player.SetPlayer(GetHealthForController());

        CameraManager.Instance.SetPlayer(m_Player.m_Transform); // Camera Setup

        EnemyManager.Instance.AddEnemies(); // Add Enemies

        IsGameOver = false;

        m_EnemiesKilled = false;

        InputSystem.Instance.IsInputEnabled = true;
    }
}
