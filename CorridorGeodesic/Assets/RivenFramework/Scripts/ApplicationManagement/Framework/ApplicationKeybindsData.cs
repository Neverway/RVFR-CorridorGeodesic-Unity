//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Data structure used by ??? to find the corresponding keyhint sprites
//  related to an action binding
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neverway.Framework.ApplicationManagement
{
    [Serializable]
    public class ApplicationKeybindsData
    {
        public string actionMap; // The id of the action map to look for in the system's tied inputAction object
        public string[] action; // The id of the action from the action map the button triggers
        public string[] binding; // The id of the button the action is tied to
    }
}