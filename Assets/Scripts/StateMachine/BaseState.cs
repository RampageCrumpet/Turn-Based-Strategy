using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    public StateMachine owner;

    /// <summary>
    /// Initializes a state
    /// </summary>
    public virtual void PrepareState() { }

    //Called each update a state is active.
    public virtual void UpdateState() { }

 

    //Called when destroying a state
    public virtual void DestroyState() { }


}
