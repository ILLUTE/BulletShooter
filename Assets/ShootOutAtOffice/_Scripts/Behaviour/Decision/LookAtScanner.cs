using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decision/LookAtScanner")]
public class LookAtScanner : BaseDecision
{
    public override bool Decide(StateController stateController)
    {
        return CheckForTarget(stateController);
    }

    private bool CheckForTarget(StateController stateController)
    {
        return stateController.enemyAlarmSystem.m_TargetBody == null || stateController.m_CanChase;
    }
}
