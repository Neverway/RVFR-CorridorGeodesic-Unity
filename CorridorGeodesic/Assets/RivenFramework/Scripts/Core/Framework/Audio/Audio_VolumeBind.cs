//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Tool_UnityExtents;

public class Audio_VolumeBind: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private MeshFilter meshFilter;
    private Bounds meshBounds => meshFilter.sharedMesh.bounds;
    private Bounds correctedBounds;
    //[SerializeField] private MeshCollider meshCollider;
    //private Tool_BaryCentricDistance baryCentricDistance;
    //private BaryCentricResult result;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        //baryCentricDistance = new Tool_BaryCentricDistance(meshFilter);
    }
    private void Update()
    {
        if (!Camera.main)
            return;

        correctedBounds = meshBounds;

        Vector3 trueSize = new Vector3(
            correctedBounds.size.x * meshFilter.transform.localScale.x,
            correctedBounds.size.y * meshFilter.transform.localScale.y,
            correctedBounds.size.z * meshFilter.transform.localScale.z);

        correctedBounds.size = trueSize;

        correctedBounds.center += meshFilter.transform.position;

        if (!correctedBounds.Contains(Camera.main.transform.position))
            transform.position = correctedBounds.ClosestPoint(Camera.main.transform.position);
        else
            transform.position = Camera.main.transform.position;
        //if(Tool_IsInMeshCollider.IsInsideMeshCollider(meshCollider, Camera.main.transform.position))
        //{
        //    transform.position = Camera.main.transform.position;
        //    return;
        //}
        //result = baryCentricDistance.GetClosestTriangleAndPoint(Camera.main.transform.position);

        //transform.position = result.closestPoint;
    }
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
