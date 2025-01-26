//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neverway.Framework.Cartographer
{
    public class WB_LevelEditor_MemoryBrowser_GroupButton : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public string category;
        public string group;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
        // Called when the button is pressed
        public void SetMemoryBrowserFilter()
        {
            FindObjectOfType<WB_LevelEditor_MemoryBrowser>().SetCurrentCategoryAndGroup(category, group);
            // Highlight the current button
            GetComponent<Image>().color = new Color(1, 1, 1, 0.1529412f);
        }
    }
}