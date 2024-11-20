//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Assigns the widget this is attached on to draw last on the UI
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
    public class Widget_Depth : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=

        private void Update()
        {
            MoveToBottomOfHierarchy();
        }

        public void MoveToBottomOfHierarchy()
        {
            // Check if the object has a parent
            if (transform.parent != null)
            {
                // Move this object to the last sibling position (bottom of the hierarchy)
                transform.SetSiblingIndex(transform.parent.childCount - 1);
            }
            else
            {
                Debug.LogWarning("This object does not have a parent.");
            }
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}