using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Laser buttonAlarm;

    private void Update()
    {
        if(buttonAlarm.m_TargetBody)
        {
            GameManager.Instance.RaiseDoorReached();
        }
    }
}
