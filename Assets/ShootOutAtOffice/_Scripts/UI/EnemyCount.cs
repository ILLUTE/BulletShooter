using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyCount : MonoBehaviour
{
    public TextMeshProUGUI m_enemyCount;
    private void Awake()
    {
        EnemyManager.OnEnemyAdded += UpdateUI;
        EnemyManager.OnEnemyRemoved += UpdateUI;
    }

    private void UpdateUI(int number)
    {
        m_enemyCount.text = number.ToString();
    }
}
