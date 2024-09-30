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

public class TMP_Timer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    private TMP_Text text;
    private bool isRunInvalid = false;

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
        isRunInvalid = false;
    }

    private void Update()
    {
        if (stopwatch && text)
        {
            text.text = $"{stopwatch.timer.Elapsed}" + (isRunInvalid ? " *?" : "!");
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
    public void InvalidateTimer()
    {
        isRunInvalid = true;
    }
}
