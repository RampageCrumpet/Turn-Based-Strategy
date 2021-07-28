using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState;



    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.DestroyState();
        }

        currentState = newState;

        if(currentState != null)
        {
            currentState.owner = this;
            currentState.PrepareState();
        }
    }
}
