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
public class Prop_Fizzler: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Transform fizzlerStart;
    [SerializeField] private Transform fizzlerEnd;
    [SerializeField] private Transform fizzler;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if(fizzlerStart && fizzlerEnd && fizzler)
        {
            fizzler.position = (fizzlerStart.position + fizzlerEnd.position) * 0.5f;
            fizzler.localScale = new Vector3(Vector3.Distance(fizzlerStart.position, fizzlerEnd.position), 4, 1);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
