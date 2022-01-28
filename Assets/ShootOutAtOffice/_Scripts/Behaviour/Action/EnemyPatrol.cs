using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Patrol")]
public class EnemyPatrol : BaseAction
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.enemyAgent.stoppingDistance = 0.5f;

        controller.shootRig.weight -= (1 / controller.m_AimTime) * Time.deltaTime;

        controller.enemyAgent.destination = (controller.patrolPoints[controller.nextWayPointIndex].position);

        Vector3 movement = ((controller.enemyAgent.steeringTarget) - (controller.enemyTransform.position));

        float turnAngle = Mathf.Atan2(movement.normalized.x, movement.normalized.z) * Mathf.Rad2Deg;

        float turnVelocity = 0;

        float angle = Mathf.SmoothDampAngle(controller.enemyTransform.eulerAngles.y, turnAngle, ref turnVelocity, 0.05f);

        controller.enemyTransform.rotation = Quaternion.Euler(0, angle, 0);

        controller.enemyAgent.isStopped = false;

        if (controller.enemyAgent.remainingDistance <= controller.enemyAgent.stoppingDistance && !controller.enemyAgent.pathPending)
        {
            if (controller.m_IdleTime > 0)
            {
                controller.enemyAgent.speed -= 3 * Time.deltaTime;

                controller.m_IdleTime -= Time.deltaTime;

                if (controller.m_IdleTime < 1.2f)
                {
                    if (controller.m_IdleTime <= 0)
                    {
                        controller.enemyTransform.rotation = Quaternion.Euler(0, turnAngle, 0);

                        controller.nextWayPointIndex = (controller.nextWayPointIndex + 1) % controller.patrolPoints.Count;

                        controller.m_IdleTime = 3;

                        controller.enemyAgent.speed = controller.agentWalkSpeed;
                    }
                }
            }
        }
    }
}

