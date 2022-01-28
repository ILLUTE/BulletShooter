using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("References")]
    public LayerMask targetLayer;

    public Color rayColour;

    public Collider m_TargetBody { get; private set; }

    private Collider[] m_TotalEnemy = new Collider[15];

    private List<Collider> cols = new List<Collider>();

    private Transform m_CurrentBody;

    public float ChecksPerSecond = 60;

    public string m_CheckTag = "Obstacle";
    [Range(0, 10)]
    public float alarmTriggerRadius = 4.5f;

    [Range(0,180)]
    public float inSightRadius = 60;

    [Header("Variables")]
    private int foundEnemies;

    private float lastCheckTime;

    private void Awake()
    {
        if (m_CurrentBody == null)
        {
            m_CurrentBody = this.GetComponent<Transform>();
        }
    }

    private void Update()
    {
        if (Time.time > lastCheckTime + (1 / ChecksPerSecond))
        {
            NearbyCheck();

            lastCheckTime = Time.time;
        }
    }

    private void NearbyCheck()
    {
        foundEnemies = Physics.OverlapSphereNonAlloc(m_CurrentBody.position, alarmTriggerRadius, m_TotalEnemy, targetLayer);

        ObstacleCheck();
    }

    private void ObstacleCheck()
    {
        cols.Clear();

        Vector3 direction;

        for (int x = 0; x < foundEnemies; x++)
        {
            direction = (m_TotalEnemy[x].transform.position - m_CurrentBody.position);

            if (Physics.Raycast(m_CurrentBody.position, direction.normalized * direction.magnitude, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(m_CheckTag))
                {
                    float angle = Vector3.Angle(direction, m_CurrentBody.forward);

                    if (angle <= inSightRadius)
                    {
                        cols.Add(m_TotalEnemy[x]);
                    }
                }
            }
        }

        if (cols.Count > 0)
        {
            CheckForNearestTarget();
        }
        else
        {
            m_TargetBody = null;
        }
    }

    private void CheckForNearestTarget()
    {
        Collider target_min = null;

        target_min = cols[0];

        for (int i = 0; i < cols.Count; i++)
        {
            float dir = (m_CurrentBody.position - cols[i].transform.position).magnitude;

            float lastNearest = (m_CurrentBody.position - target_min.transform.position).magnitude;

            if (dir < lastNearest)
            {
                target_min = cols[i];
            }
        }

        m_TargetBody = target_min;
    }

    public bool Remove(Collider[] x, int length, Collider collider)
    {
        int index = -1;

        bool removed = false;

        for (int i = 0; i < length; i++)
        {
            if (x[i] == collider)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            removed = true;
            for (int i = index; i < length - 1; i++)
            {
                x[i] = x[i + 1];
            }
        }

        return removed;
    }

    private void RemoveAt(int index, int length)
    {
        for (int i = index; i < length - 1; i++)
        {
            m_TotalEnemy[i] = m_TotalEnemy[i + 1];
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (m_CurrentBody != null)
        {
            Gizmos.DrawWireSphere(m_CurrentBody.position, alarmTriggerRadius);
            Gizmos.color = Color.blue;
            Methods.DrawWireArc(m_CurrentBody.position, m_CurrentBody.forward, inSightRadius*2, alarmTriggerRadius);
            Gizmos.color = rayColour;

            if (m_TargetBody != null)
            {
                Vector3 direction = (m_TargetBody.transform.position - m_CurrentBody.position);
                Gizmos.DrawRay(m_CurrentBody.position, direction.normalized * direction.magnitude);
            }
        }
    }
#endif
}
