//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public System.Diagnostics.Stopwatch timer;


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
    
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartTimer()
    {
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
}
