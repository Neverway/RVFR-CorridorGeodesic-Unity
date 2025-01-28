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
    public bool clearIfNoTeslaPowerSource = true;

    //=-----------------=
    // Private Variables
    //=-----------------=

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [HideInInspector] public TeslaPowerSource source1;
    [HideInInspector] public TeslaPowerSource source2;
    
    public LineRenderer lineRenderer;
    public Transform endTransform;
    public float jitter = 0.12f;

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
        Update();
    }

    private void Update()
    {
        if (clearIfNoTeslaPowerSource)
        {
            if (source1 == null || source2 == null || !(source1.IsTeslaPowered() && source2.IsTeslaPowered()))
            {
                Destroy(gameObject);
                return;
            }
        }


        int newTime = Mathf.RoundToInt(Time.time * 16f);
        if (newTime == lastTime)
            return;

        lastTime = newTime;

        SetPoints();
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

        lineRenderer.positionCount = Mathf.RoundToInt(diff.magnitude) + 1;
        points = new Vector3[lineRenderer.positionCount];
        noisyPoints = new Vector3[lineRenderer.positionCount];

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
    public void SetStartAndEndPoints(Vector3 start, Vector3 end)
    {
        transform.position = start;

        endTransform.position = end;
        Update();
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}