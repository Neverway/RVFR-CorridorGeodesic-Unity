//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        InvokeRepeating(nameof(UpdateActiveFonts), 0, 1);
    }

    private void UpdateActiveFonts()
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
