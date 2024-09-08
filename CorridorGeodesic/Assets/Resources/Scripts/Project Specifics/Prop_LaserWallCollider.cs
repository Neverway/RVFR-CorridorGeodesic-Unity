//===================== (Neverway 2024) Written by Soulex =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_LaserWallCollider: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public BoxCollider boxCollider;
    public LineRenderer lineRenderer;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ChangeValues(Vector3 boxSize, Vector3 boxCenter, Vector3 lineStart, Vector3 lineEnd, float lineWidth)
    {
        lineRenderer.positionCount = 2;

        boxCollider.size = boxSize;
        boxCollider.center = boxCenter;

        lineRenderer.SetPosition(0, lineStart);
        lineRenderer.SetPosition(1, lineEnd);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    public void ResetValues()
    {
        lineRenderer.positionCount = 2;

        boxCollider.size = Vector3.zero;
        boxCollider.center = Vector3.zero;

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;
    }
}
