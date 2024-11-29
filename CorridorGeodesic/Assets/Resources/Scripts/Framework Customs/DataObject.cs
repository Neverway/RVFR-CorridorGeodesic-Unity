//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Base class to extend, used for linking an object id with data related
//  to the object, like an item to get its stats and icon, this is extended by Actor
// Notes:
//
//=============================================================================

using System;
using UnityEngine;
using Neverway.Framework;

namespace Neverway.Framework
{
    [Serializable]
    public class DataObject : ScriptableObject
    {
        public string id;
        public Sprite icon;
    }
}