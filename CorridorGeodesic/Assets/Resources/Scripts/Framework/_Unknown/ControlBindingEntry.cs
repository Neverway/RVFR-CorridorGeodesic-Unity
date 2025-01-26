//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using TMPro;
using UnityEngine;

namespace Neverway.Framework.ApplicationManagement
{
    public class ControlBindingEntry : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public TMP_Text text;
        public Image_KeyHint[] keyHints;
        public bool isComposite;


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
        public void SendRebindRequest()
        {
            Debug.Log($"[{this.name}] Executing function 'SendRebindRequest()'");
            FindObjectOfType<WB_Settings_Controls>()
                .Rebind(keyHints[0].targetActionMap, keyHints[0].targetAction, isComposite);
        }
    }
}