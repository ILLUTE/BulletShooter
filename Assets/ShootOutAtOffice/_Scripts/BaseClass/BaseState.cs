using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class BaseState : ScriptableObject
{
    public BaseAction[] actions;
    public BaseTransition[] transitions;

    public void UpdateState(StateController stateController)
    {
        DoActions(stateController);
        CheckDecisions(stateController);
    }

    private void DoActions(StateController stateController)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(stateController);
        }
    }

    private void CheckDecisions(StateController stateController)
    {
        bool decision;

        for (int i = 0; i < transitions.Length; i++)
        {
            decision = transitions[i].decision.Decide(stateController);

            ChangeState(stateController,decision ? transitions[i].trueState : transitions[i].falseState);
        }
    }

    private void ChangeState(StateController controller,BaseState state)
    {
        controller.ChangeState(state);
    }
}
