//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestRiftPreview: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Material mat;
    [Range(0, 1)] public float effectTime;
    public Transform bulbPos;

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
        if (!mat || !bulbPos)
            return;

        mat.SetFloat("_EffectTime", effectTime);
        mat.SetVector("_BulbsCenter", bulbPos.position);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
