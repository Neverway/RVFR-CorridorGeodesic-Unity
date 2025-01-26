//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_SimpleLine: MonoBehaviour
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
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform[] linePoints;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        lineRenderer.positionCount = linePoints.Length;
        lineRenderer.useWorldSpace = true;
    }
    private void LateUpdate()
    {
        for (int i = 0; i < linePoints.Length; i++)
        {
            lineRenderer.SetPosition(i, linePoints[i].position);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
