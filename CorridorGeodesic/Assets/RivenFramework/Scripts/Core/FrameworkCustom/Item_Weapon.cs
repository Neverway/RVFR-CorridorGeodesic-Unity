//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
    [CreateAssetMenu(fileName = "Item_Weapon", menuName = "Neverway/ScriptableObjects/Items/Weapon")]
    public class Item_Weapon : Item
    {
        public float damage;
        public Vector2 knockbackForce;
        public float knockbackForceDuration;
        public string effect;
    }
}

