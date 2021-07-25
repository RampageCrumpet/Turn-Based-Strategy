using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    [SerializeField]
    [Tooltip("The name of the weapon to be displayed to the player.")]
    string weaponName = "UnnamedWeapon";

    [SerializeField]
    [Tooltip("The minimum number of tiles away a target needs to be for a weapon to function.")]
    int minimumRange = 0;

    [SerializeField]
    [Tooltip("The maximum number of tiles away a target can be for a weapon to target it.")]
    int maximumRange = 1;

    [SerializeField]
    [Tooltip("The unit types this weapon is allowed to attack.")]
    Unit.UnitType weaponTargets;
}
