using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetectors : MonoBehaviour
{
    public LineRenderer laserDetection;

    public Transform spawnReference;

    public float laserDamage = 10;

    private Ray m_Ray;
    private RaycastHit hit;

    private float waitForPlayerCheck = 0.0f;

    private bool laserOn = false;

    private float laserOnTimer = 2.5f;

    private void Update()
    {
        if (waitForPlayerCheck > 0)
        {
            waitForPlayerCheck -= Time.deltaTime;
        }

        m_Ray.origin = spawnReference.position;
        m_Ray.direction = spawnReference.forward;

        LaserSetPoint(0, spawnReference.position);

        if (Physics.Raycast(m_Ray, out hit))
        {
            LaserSetPoint(1, hit.point);

            if (hit.collider.CompareTag("Player"))
            {
                if (waitForPlayerCheck <= 0)
                {
                    PlayerDetected(hit.collider.GetComponent<HealthPoints>());
                    waitForPlayerCheck = 1.0f;
                }
            }
        }
    }

    private void ToggleLaser()
    {
        laserOn = !laserOn;
        laserOnTimer = 2.5f;

        laserDetection.positionCount = 0;
    }

    public void PlayerDetected(HealthPoints hp)
    {
        hp.CurrentHealthUpdate(-1 * laserDamage);
        CameraManager.Instance.Shake(1.5f, 0.2f);
    }

    private void LaserSetPoint(int pos, Vector3 ref_pos)
    {
        if (laserDetection.positionCount < pos)
        {
            laserDetection.positionCount = pos + 1;
        }

        laserDetection.SetPosition(pos, ref_pos);
    }
}
