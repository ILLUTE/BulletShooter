using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Color spawnColor;
    public Transform m_Spawn;
    public bool IsOccupied { get; set; }
    public GameObject spawnAssigned;

    public Transform[] m_PatrolPoints;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = spawnColor;
        Gizmos.DrawWireSphere(transform.position, 1);
        Gizmos.color = spawnColor * 0.5f;
        for (int i = 0; i < m_PatrolPoints.Length; i++)
        {
            if (m_PatrolPoints[i] != null)
            {
                Gizmos.DrawWireCube(m_PatrolPoints[i].position, Vector3.one * 0.3f);
            }
        }
    }
#endif
}
