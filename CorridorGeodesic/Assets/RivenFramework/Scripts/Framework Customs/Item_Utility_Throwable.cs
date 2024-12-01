//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
    [CreateAssetMenu(fileName = "Item_Utility_Throwable", menuName = "Neverway/ScriptableObjects/Items/Throwable")]
    public class Item_Utility_Throwable : Item
    {
        public float baseDamage;
        public Vector2 forceStrength;
        public float removeForceDelay;
        public string damageType;
        public float speed;
        public float drag;

        [Tooltip("This is an object that will be instantiated when the object collides with something")]
        public GameObject hitObject;

        [Tooltip("This is how long the object will exist for before destructing and creating the hit object")]
        public float lifetime;
    }
}

