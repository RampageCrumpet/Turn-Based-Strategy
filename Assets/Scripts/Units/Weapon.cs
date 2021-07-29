using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 1)]
[System.Serializable]
public class Weapon : ScriptableObject 
{
    [SerializeField]
    [Tooltip("The name of the weapon to be displayed to the player.")]
    public string weaponName = "UnnamedWeapon";

    [SerializeField]
    [Tooltip("The minimum number of tiles away a target needs to be for a weapon to function.")]
    public int minimumRange = 0;

    [SerializeField]
    [Tooltip("The maximum number of tiles away a target can be for a weapon to target it.")]
    public int maximumRange = 1;

    [SerializeField]
    [Tooltip("The ammount of armor this weapon can penetrate before its damage is reduced.")]
    int armorPen = 1;

    [SerializeField]
    [Tooltip("The base damage dealt by this unit.")]
    int baseDamage = 3;

    [SerializeField]
    [Tooltip("If false a weapon can only be fired if it's owner doesn't move that turn.")]
    public bool canFireAfterMoving = true;


    [Tooltip("The unit types this weapon is allowed to attack.")]
    public Unit.UnitType weaponTargets;

    


    public int CalculateDamage(int unitHp, int targetArmor, int terrainDefense)
    {
        //Calculate the total damage done.
        float damage = (unitHp * 0.1f)*((float)baseDamage)*(Mathf.Clamp((float)armorPen / (float)targetArmor, 0, 1)*(-0.1f * (float)terrainDefense));

        return Mathf.FloorToInt(damage);
    }
}
