//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningLine : MonoBehaviour
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

    [SerializeField] private Transform endTransform;
    public float jitter = 0.12f;

    private LineRenderer lineRenderer;
    private Vector3[] points;
    private Vector3[] noisyPoints;
    private Vector3 startPos;
    private Vector3 endPos;
    private int lastTime;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new Vector3[lineRenderer.positionCount];
        noisyPoints = new Vector3[lineRenderer.positionCount];
        SetPoints ();
    }

    private void Update()
    {


        if (startPos != transform.position || endPos != endTransform.position)
        {
            SetPoints();
        }

        int newTime = Mathf.RoundToInt(Time.time * 16f);
        if (newTime == lastTime)
            return;

        lastTime = newTime;

        for (int i = 1; i < points.Length-1; i++)
        {
            noisyPoints[i] = points[i] + new Vector3 (Random.Range (-jitter, jitter), Random.Range (-jitter, jitter), Random.Range (-jitter, jitter));
        }
        lineRenderer.SetPositions(noisyPoints);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void SetPoints ()
    {
        startPos = transform.position;
        endPos = endTransform.position;
        Vector3 diff = endPos - startPos;
        Vector3 forward = diff.magnitude / (lineRenderer.positionCount-1) * diff.normalized;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            points[i] = startPos + (forward * i);
        }
        noisyPoints[0] = points[0];
        noisyPoints[points.Length-1] = points[points.Length-1];
    }

    [ContextMenu("UpdatePoints")]
    public void UpdatePoints()
    {
        lastTime = -1;
        Start();
        Update();
        lastTime = 0;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}