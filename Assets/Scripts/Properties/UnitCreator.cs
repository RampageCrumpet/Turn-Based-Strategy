using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : Installation
{
    [SerializeField]
    [Tooltip("The list of unit types that can be produced at this ")]
    Unit.UnitType UnitTemplates;
}
