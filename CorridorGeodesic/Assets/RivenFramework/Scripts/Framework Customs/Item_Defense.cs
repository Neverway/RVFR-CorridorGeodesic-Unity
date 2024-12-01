//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework
{

    [CreateAssetMenu(fileName = "Item_Defense", menuName = "Neverway/ScriptableObjects/Items/Defense")]
    public class Item_Defense : Item
    {
        [Tooltip("The amount of damage that will be negated by default when this defense item is equipped")]
        public float baseDefense;

        [Tooltip(
            "If the damage type is one that this defense item is resistant to, multiply the defense stat by this amount")]
        public float resistantMultiplier;

        [Tooltip(
            "If the damage type is one that this defense item is weak to, multiply the defense stat by this amount (should be less than one to weaken the base defense)")]
        public float criticalMultiplier;

        [Tooltip("The damage types that this defense item is strong against")]
        public List<string> resistantTypes;

        [Tooltip("The damage types that this defense item is weak against")]
        public List<string> criticalTypes;
    }
}

