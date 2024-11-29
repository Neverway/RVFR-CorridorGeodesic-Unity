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

public class Func_Timer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private TMP_Text text;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Stopwatch stopwatch;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        text = GetComponent<TMP_Text>();
        stopwatch = FindObjectOfType<Stopwatch>();
    }

    private void Update()
    {
        if (stopwatch && text)
        {
            text.text = $"{stopwatch.timer.Elapsed}" + (stopwatch.isInvalid ? "?" : "!");
        }
        else
        {
            text = GetComponent<TMP_Text>();
            stopwatch = FindObjectOfType<Stopwatch>();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartTimer()
    {
        stopwatch.ResetTimer();
        stopwatch.StartTimer();
    }
    public void StopTimer()
    {
        stopwatch.StopTimer();
    }
}
