//===================== (Neverway 2024) Written by Liz M. & Connorses =====================
//
// Purpose: Split a mesh into three slices and remove the center slice
// Notes: This code was "Super Expertly Adapted" from the source code created by
//          @DitzelGames on YouTube. (See source)
//          Also thanks to Connorses for helping fix the bridgeMeshGaps function
//          and for putting up with my crazed rambling about polygons. ~Liz
// Source: https://www.youtube.com/watch?v=VwGiwDLQ40A
//
//=============================================================================

using System;
using BzKovSoft.ObjectSlicer;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BzSliceableObject))]
public class ALTMeshSlicer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip ("If true, this object's collider reflects lasers.")]
    public bool isReflective = false;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool isCut = false; // Debug value to keep track of if this is a cut object or not
    private bool destroyOriginal;
    private Vector3 homePosition;
    private Vector3 homeScale;
    private Quaternion homeRotation;
    private Transform homeParent;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private BzSliceableObject sliceableObject;
    private IBzMeshSlicer meshSlicer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        meshSlicer = GetComponent<IBzMeshSlicer> ();
        sliceableObject = GetComponent<BzSliceableObject> ();
    }
    
    public void Update()
    {
        foreach (var meshCollider in gameObject.GetComponents<MeshCollider>())
        {
            meshCollider.convex = false;
            meshCollider.sharedMesh = meshCollider.sharedMesh;
        }
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=

    
    //=-----------------=
    // External Functions
    //=-----------------=
    public void ApplyCuts()
    {
        ALTMeshSlicer sliceThis = Instantiate (this, transform.position, transform.rotation);
        sliceThis.gameObject.name = $"[CUT] {name}";
        isCut = false;
        ALTItem_Geodesic_Utility_GeoFolder.originalSliceableObjects.Add(this);
        homePosition = transform.position;
        homeScale = transform.localScale;
        homeParent = transform.parent;
        homeRotation = transform.rotation;
        sliceThis.SliceClone (gameObject);
    }

    private async void SliceClone(GameObject _original)
    {
        bool sliced = false;
        Collider coll = GetComponent<Collider> ();
        if (!coll) return;
        bool isTrigger = coll.isTrigger;
        sliceableObject= GetComponent<BzSliceableObject> ();
        //Slice the object
        var result = await sliceableObject.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.planeA, meshSlicer);
        if (result.sliced)
        {

            _original.SetActive (false);
            sliced = true;
            foreach (var obj in result.resultObjects)
            {
                obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                foreach (Collider collider in obj.gameObject.GetComponents<Collider> ())
                {
                    collider.isTrigger = isTrigger;
                }
                
                ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                if (obj.side)
                {
                    //Slice the new object on the positive side of the cut, this time with the other plane
                    IBzMeshSlicer objSlicer = obj.gameObject.GetComponent<IBzMeshSlicer> ();
                    var result2 = await objSlicer.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.planeB);
                    if (result2.sliced)
                    {
                        sliced = true;
                        _original.SetActive (false);
                        //add the positive sides to the null list
                        foreach (var obj2 in result2.resultObjects)
                        {
                            foreach (Collider collider in obj2.gameObject.GetComponents<Collider> ())
                            {
                                collider.isTrigger = isTrigger;
                            }
                            if (obj2.gameObject.TryGetComponent<Collider> (out coll))
                            {
                                coll.isTrigger = isTrigger;
                            }
                            obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                            ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                            if (obj2.side)
                            {
                                ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (obj2.gameObject);
                            }
                            else
                            {
                                obj2.gameObject.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.planeBMeshes.transform);
                            }
                        }
                    }
                    else
                    {
                        //if slice 2 failed, we still add this object
                        ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (obj.gameObject);
                    }
                }
            }
        }

        else //if we didn't slice, we still try slicing with plane2
        {
            var result2 = await sliceableObject.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.planeB, meshSlicer);
            if (result2.sliced)
            {
                _original.SetActive (false);
                sliced = true;
                foreach (var obj in result2.resultObjects)
                {
                    foreach (Collider collider in obj.gameObject.GetComponents<Collider> ())
                    {
                        collider.isTrigger = isTrigger;
                    }
                    obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                    ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                    if (obj.side)
                    {
                        ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (obj.gameObject);
                    }
                    else
                    {
                        obj.gameObject.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.planeBMeshes.transform);
                    }
                }
            }
        }


        if (!sliced)
        {
            //If all the slices miss, we have to figure out where this mesh is.

            MeshFilter meshFilter = GetComponent<MeshFilter> ();
            if (meshFilter != null)
            {
                var vert = meshFilter.mesh.vertices[0];
                Vector3 testPoint = new Vector3 (vert.x, vert.y, vert.z);
                Vector3 worldPoint = transform.TransformPoint (testPoint);
                if (ALTItem_Geodesic_Utility_GeoFolder.planeA.GetDistanceToPoint (worldPoint) < 0)
                {
                    //Entire object was outside nullspace plane1
                    _original.gameObject.SetActive (true);
                    Destroy (gameObject);
                    return;
                }
                else
                {
                    if (ALTItem_Geodesic_Utility_GeoFolder.planeB.GetDistanceToPoint (worldPoint) < 0)
                    {
                        //entire object was outside nullspace plane2
                        _original.SetActive (true);
                        _original.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.planeBMeshes.transform);
                        Destroy (gameObject);
                        return;
                    }
                    else
                    {
                        //Entire object was in the nullspace
                        ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (_original);
                        Destroy (gameObject);
                        return;
                    }
                }
            }
        }
    }

    public void GoHome()
    {
        transform.SetParent(homeParent);
        gameObject.SetActive (true);
        transform.position = homePosition;
        transform.localScale = homeScale;
        transform.rotation = homeRotation;
    }

}

