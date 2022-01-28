using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTransition
{
    public BaseDecision decision;
    public BaseState trueState;
    public BaseState falseState;
}
