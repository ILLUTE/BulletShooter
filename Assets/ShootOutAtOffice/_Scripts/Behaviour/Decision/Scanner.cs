using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decision/Scanner")]
public class Scanner : BaseDecision
{
    public override bool Decide(StateController stateController)
    {
        return CheckForTarget(stateController);
    }

    private bool CheckForTarget(StateController stateController)
    {
        return stateController.enemyAlarmSystem.m_TargetBody != null;
    }
}
