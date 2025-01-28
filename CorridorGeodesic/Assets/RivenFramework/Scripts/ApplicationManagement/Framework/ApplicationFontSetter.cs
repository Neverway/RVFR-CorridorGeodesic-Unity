//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Defines a consistent default font to all text elements and allows
//  them to be replaced by the dyslexia friendly font
// Notes:
//
//=============================================================================

using TMPro;
using UnityEngine;

namespace Neverway.Framework.ApplicationManagement
{
    public class ApplicationFontSetter : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public int currentFont;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        public TMP_FontAsset defaultFont, dyslexiaAssistFont;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            InvokeRepeating(nameof(UpdateFonts), 0, 0.25f);
        }

        private void UpdateFonts()
        {
            TMP_FontAsset targetFont = null;
            switch (currentFont)
            {
                case 0:
                    targetFont = defaultFont;
                    break;
                case 1:
                    targetFont = dyslexiaAssistFont;
                    break;
                default:
                    targetFont = defaultFont;
                    break;
            }

            foreach (var textElement in FindObjectsOfType<TMP_Text>())
            {
                if (textElement.gameObject.GetComponent(typeof(Text_DontOverideFont))) continue;
                textElement.font = targetFont;
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