using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDecision : ScriptableObject
{
    public abstract bool Decide(StateController stateController);
}
