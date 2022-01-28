using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
            }

            return instance;
        }
    }

    public static event Action OnEnemiesCleared;
    public static event Action<int> OnEnemyAdded;
    public static event Action<int> OnEnemyRemoved;

    [SerializeField]
    private List<SpawnPoint> m_EnemySpawns = new List<SpawnPoint>();
    private List<StateController> m_TotalEnemies = new List<StateController>();

    public void AddEnemies()
    {
        StateController temp;
        SpawnPoint spawn;

        for (int i = 0; i < m_EnemySpawns.Count; i++)
        {
            spawn = GetSpawn();

            if(spawn == null)
            {
                Debug.LogError("Got a null spawn");
                continue;
            }
            spawn.IsOccupied = true;

            temp = Instantiate(ResourceManager.Instance.r_Enemy);
            temp.enemyTransform.position = spawn.m_PatrolPoints[0].position;
            temp.SetPatrolPoints(GameManager.Instance.GetHealthForController(), spawn.m_PatrolPoints, spawn.m_PatrolPoints.Length);
            m_TotalEnemies.Add(temp);
        }

        OnEnemyAdded?.Invoke(m_TotalEnemies.Count);
    }

    public void DestroyEnemies(Action callback)
    {
        RemoveSpawnOccupied();

        StartCoroutine(DestroyEnemiesCoroutine(() =>
        {
            m_TotalEnemies.Clear();

            OnEnemyRemoved?.Invoke(m_TotalEnemies.Count);

            callback?.Invoke();
        }));
    }

    private IEnumerator DestroyEnemiesCoroutine(System.Action callback)
    {
        StateController[] list = FindObjectsOfType<StateController>();

        for (int i = 0; i < list.Length; i++)
        {
            Destroy(list[i].gameObject);

            yield return null;
        }

        callback?.Invoke();
    }



    public void OnEnemyKilled(StateController enemy)
    {
        m_TotalEnemies.Remove(enemy);

        OnEnemyRemoved?.Invoke(m_TotalEnemies.Count);

        if (m_TotalEnemies.Count <= 0)
        {
            OnEnemiesCleared?.Invoke();
        }
    }

    private void RemoveSpawnOccupied()
    {
        for (int i = 0; i < m_EnemySpawns.Count; i++)
        {
            m_EnemySpawns[i].IsOccupied = false;
        }
    }

    private SpawnPoint GetSpawn()
    {
        int random = UnityEngine.Random.Range(0, m_EnemySpawns.Count);

        while (m_EnemySpawns[random].IsOccupied)
        {
            random = UnityEngine.Random.Range(0, m_EnemySpawns.Count);
        }

        return m_EnemySpawns[random];
    }
}
