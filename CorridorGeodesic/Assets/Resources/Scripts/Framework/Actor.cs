//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Base class to extend, used for linking an actor id with data related
//  to the actor, like an item to get its stats and icon
// Notes: (Game Object, Entity) Object that can be placed in a level
//
//=============================================================================

using UnityEngine;


namespace Neverway.Framework
{
    public class Actor : DataObject
    {
        public string actorName;
        public GameObject AssociatedGameObject;
    }
}