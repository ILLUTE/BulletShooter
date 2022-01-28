using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/EnemySuspiciousLook")]
public class EnemySuspiciousLook : BaseAction
{
    public override void Act(StateController controller)
    {
        controller.enemyAgent.isStopped = true;
        controller.enemyAgent.speed -= Time.deltaTime;

        if (controller.m_AllowSuspiciousTime > 0)
        {
            controller.m_AllowSuspiciousTime -= Time.deltaTime;

            if (controller.m_AllowSuspiciousTime <= 0)
            {
                controller.m_CanChase = true;

                controller.m_AllowSuspiciousTime = 1.0f;
            }
        }
    }
}
