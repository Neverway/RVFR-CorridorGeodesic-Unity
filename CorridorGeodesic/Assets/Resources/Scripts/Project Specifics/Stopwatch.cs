//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public System.Diagnostics.Stopwatch timer;

    [DebugReadOnly] public bool isInvalid = false;
    [DebugReadOnly] public double timeElapsed;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
    }

    private void Update()
    {
        timeElapsed = timer.Elapsed.TotalSeconds;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartTimer()
    {
        isInvalid = false;
        timer.Start();
    }
    public void StopTimer()
    {
        timer.Stop();
    }
    public void ResetTimer()
    {
        timer.Restart();
    }
    public void InvalidateTimer()
    {
        isInvalid = true;
    }
}
