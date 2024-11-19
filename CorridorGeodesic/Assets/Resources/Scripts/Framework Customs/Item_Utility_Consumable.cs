//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
    [CreateAssetMenu(fileName = "Item_Utility_Consumable", menuName = "Neverway/ScriptableObjects/Items/Consumable")]
    public class Item_Utility_Consumable : Item
    {
        public string effect;
        public float amount;
    }
}

