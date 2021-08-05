using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability
{
    protected Unit owner;
    string name = "Unamed Ability";

    public virtual void Initialize(Unit owner)
    {
        this.owner = owner;
    }

    public abstract bool CanExecute();

    public abstract void Execute();
}
