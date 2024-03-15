//========== Neverway 2023 Project Script | Written by Unknown Dev ============
// 
// Type: 
// Purpose: 
// Applied to: 
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item_Defense", menuName="Neverway/ScriptableObjects/Items/Defense")]
public class Item_Defense : Item
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("The amount of damage that will be negated by default when this defense item is equipped")]
    public float baseDefense;
    [Tooltip("If the damage type is one that this defense item is resistant to, multiply the defense stat by this amount")]
    public float resistantMultiplier;
    [Tooltip("If the damage type is one that this defense item is weak to, multiply the defense stat by this amount (should be less than one to weaken the base defense)")]
    public float criticalMultiplier;
    [Tooltip("The damage types that this defense item is strong against")]
    public List<string> resistantTypes;
    [Tooltip("The damage types that this defense item is weak against")]
    public List<string> criticalTypes;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}

