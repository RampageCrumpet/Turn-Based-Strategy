using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds all of the start information required by the player.
/// </summary>
/// 

[System.Serializable]
public class PlayerStartState 
{
    [field: Tooltip("The money the player currently has.")]
    [field: SerializeField]
    public int Money { get; private set; } = 0;

    [SerializeField]
    [Tooltip("The units the player owns.")]
    public List<Unit> playerUnits = new List<Unit>();

    [SerializeField]
    [Tooltip("The installations this payer owns.")]
    public List<Installation> playerInstallations = new List<Installation>();
}
