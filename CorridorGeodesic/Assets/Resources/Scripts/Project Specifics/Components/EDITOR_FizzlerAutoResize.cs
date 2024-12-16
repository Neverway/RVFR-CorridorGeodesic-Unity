//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EDITOR_FizzlerAutoResize : MonoBehaviour
{
#if UNITY_EDITOR
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Transform fizzlerStart;
    [SerializeField] private Transform fizzlerEnd;
    [SerializeField] private Transform fizzler;
    [SerializeField] private float thickness = 0.025f;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (Application.isPlaying)
            return;

        if(fizzlerStart && fizzlerEnd && fizzler)
        {
            fizzler.position = (fizzlerStart.position + fizzlerEnd.position) * 0.5f;
            fizzler.localScale = new Vector3(Vector3.Distance(fizzlerStart.position, fizzlerEnd.position), 3.9f, thickness);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
#endif
}
