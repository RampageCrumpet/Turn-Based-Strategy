using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: This should probbably be converted to a virtual class. 
[CreateAssetMenu(fileName = "Commander", menuName = "Commanders/Commander", order = 1)]
public class Commander : ScriptableObject                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
{
    [field: SerializeField]
    [field: Tooltip("The unit Prefabs a player can create.")]
    public List<Unit> ConstructionPrefabs { get; private set; } = new List<Unit>();
}
