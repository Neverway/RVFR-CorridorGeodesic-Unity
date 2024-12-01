//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Updates a TextMeshPro text component with the current project version.
// Notes: This script assumes TextMeshPro is used for displaying text.
//
//=============================================================================

using TMPro;
using UnityEngine;

namespace Neverway.Framework
{
    public class Text_ProjectVersion : MonoBehaviour
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
        private TMP_Text tmpText;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            tmpText = GetComponent<TMP_Text>();
            tmpText.text = Application.version;
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}