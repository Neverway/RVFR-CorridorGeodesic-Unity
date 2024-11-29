//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

namespace Neverway.Framework
{
    [Serializable]
    public class Item : Actor
    {
        public int stackCount;
        [TextArea] public string description;
        public bool isDiscardable = true;

        [Header("0 - Utility, 1 - Weapon, 2 - Magic, 3 - Defense")]
        public int category;
    }
}