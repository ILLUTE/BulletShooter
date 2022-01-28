using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/StupidChase")]
public class EnemyChase : BaseAction
{
    public override void Act(StateController controller)
    {
        ChaseEnemy(controller);
    }

    private void ChaseEnemy(StateController controller)
    {
        if (controller.enemyAlarmSystem.m_TargetBody == null)
        {
            return;
        }

        controller.enemyAgent.stoppingDistance = 2f;

        controller.shootRig.weight += (1 / controller.m_AimTime) * Time.deltaTime;

        Vector3 movement = ((controller.enemyAlarmSystem.m_TargetBody.transform.position) - (controller.enemyTransform.position));

        float turnAngle = Mathf.Atan2(movement.normalized.x, movement.normalized.z) * Mathf.Rad2Deg;

        controller.enemyTransform.rotation = Quaternion.Euler(0, turnAngle, 0);

        controller.enemyAgent.destination = (controller.enemyAlarmSystem.m_TargetBody.transform.position);

        if(controller.enemyAgent.remainingDistance <= controller.enemyAgent.stoppingDistance && !controller.enemyAgent.pathPending)
        {
            controller.enemyAgent.isStopped = true;

            controller.enemyAgent.speed -= 30 * Time.deltaTime;

            controller.enemyAgent.speed = Mathf.Clamp(controller.enemyAgent.speed, 0, controller.agentRunSpeed);

            controller.shootRig.weight = 1;

            controller.Shoot(controller.enemyAlarmSystem.m_TargetBody.transform);
        }
        else
        {
            controller.enemyAgent.isStopped = false;

            controller.enemyAgent.speed = controller.agentRunSpeed;
        }
    }
}
