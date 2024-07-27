//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Split a mesh into three slices and remove the center slice
// Notes: This code was "Super Expertly Adapted" from the source code created by
//          @DitzelGames on YouTube. (See source)
//          Also thanks to Connorses for helping fix the bridgeMeshGaps function
//          and for putting up with my crazed rambling about polygons. ~Liz
// Source: https://www.youtube.com/watch?v=VwGiwDLQ40A
//
//=============================================================================

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
    [Tooltip ("Two objects in space, that use their rotation to specify where a cut will be made on this mesh")]
    public List<GameObject> cutPlanes = new List<GameObject> ();
    [Tooltip ("A variable used by other scripts to identify parts that have already been cut (Usually so we know not to cut them a second time)")]
    public bool isCascadeSegment;
    [Tooltip ("A variable used to identify which part of this mesh is once it's been cut (This is currently used so we can delete the center slice to collapse space)")]
    public int segmentId; // This value is used to keep track of what cut this is, 0 is uncut, 1 is a weird empty-cut glitch, 0 is the left slice, 2 is the right slice, 3 is the center slice

    [Header ("Debugging")]
    public bool isConvex;
    [Tooltip ("If enabled, once an object has been sliced, this will attempt to fill in the new hole created in the mesh from where it was cut")]
    public bool bridgeMeshGaps;
    [Tooltip ("If true, this object's collider reflects lasers.")]
    public bool isReflective = false;

    public bool isCut = false;


    //=-----------------=
    // Private Variables
    //=-----------------=
    //private bool edgeSet = false;
    private Vector3 edgeVertex = Vector3.zero;
    private Vector2 edgeUV = Vector2.zero;
    //private Plane edgePlane = new Plane ();
    private bool destroyOriginal;
    private List<Vector3> cutNormals = new List<Vector3> ();
    private List<float> inPointDistances = new List<float> ();
    private BzSliceableObject sliceableObject;
    private IBzMeshSlicer meshSlicer;
    private Vector3 homePosition;
    private Vector3 homeScale;
    private Quaternion homeRotation;
    private Transform homeParent;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        meshSlicer = GetComponent<IBzMeshSlicer> ();
        sliceableObject = GetComponent<BzSliceableObject> ();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=

    public void ApplyCuts ()
    {
        if (!enabled)
        {
            Debug.LogWarning ("ALTMeshSlicer must be enabled to slice! If you wanted to use the old MeshSlicer, remove this from gameObjects");
            return;
        }
        ALTMeshSlicer sliceThis = Instantiate (this, transform.position, transform.rotation);
        sliceThis.gameObject.name = name + "(Sliced)";
        isCut = false;
        ALTItem_Geodesic_Utility_GeoFolder.originalSliceableObjects.Add (this);
        homePosition = transform.position;
        homeScale = transform.localScale;
        homeParent = transform.parent;
        homeRotation = transform.rotation;
        sliceThis.SliceClone (gameObject);
    }

    public async void SliceClone (GameObject original)
    {
        bool sliced = false;
        sliceableObject= GetComponent<BzSliceableObject> ();
        //Slice the object
        var result = await sliceableObject.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.plane1, meshSlicer);
        if (result.sliced)
        {
            original.SetActive (false);
            sliced = true;
            foreach (var obj in result.resultObjects)
            {
                obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                if (obj.side)
                {
                    //Slice the new object on the positive side of the cut, this time with the other plane
                    IBzMeshSlicer objSlicer = obj.gameObject.GetComponent<IBzMeshSlicer> ();
                    var result2 = await objSlicer.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.plane2);
                    if (result2.sliced)
                    {
                        sliced = true;
                        original.SetActive (false);
                        //add the positive sides to the null list
                        foreach (var obj2 in result2.resultObjects)
                        {
                            obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                            ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                            if (obj2.side)
                            {
                                ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (obj2.gameObject);
                            }
                            else
                            {
                                obj2.gameObject.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.plane2Meshes.transform);
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
            var result2 = await sliceableObject.SliceAsync (ALTItem_Geodesic_Utility_GeoFolder.plane2, meshSlicer);
            if (result2.sliced)
            {
                original.SetActive (false);
                sliced = true;
                foreach (var obj in result2.resultObjects)
                {
                    obj.gameObject.GetComponent<ALTMeshSlicer> ().isCut = true;
                    ALTItem_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                    if (obj.side)
                    {
                        ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (obj.gameObject);
                    }
                    else
                    {
                        obj.gameObject.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.plane2Meshes.transform);
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
                if (ALTItem_Geodesic_Utility_GeoFolder.plane1.GetDistanceToPoint (worldPoint) < 0)
                {
                    //Entire object was outside nullspace plane1
                    original.gameObject.SetActive (true);
                    Destroy (gameObject);
                    return;
                }
                else
                {
                    if (ALTItem_Geodesic_Utility_GeoFolder.plane2.GetDistanceToPoint (worldPoint) < 0)
                    {
                        //entire object was outside nullspace plane2
                        original.SetActive (true);
                        original.transform.SetParent (ALTItem_Geodesic_Utility_GeoFolder.plane2Meshes.transform);
                        Destroy (gameObject);
                        return;
                    }
                    else
                    {
                        //Entire object was in the nullspace
                        ALTItem_Geodesic_Utility_GeoFolder.nullSlices.Add (original);
                        Destroy (gameObject);
                        return;
                    }
                }
            }
        }

    }

    public void GoHome ()
    {
        transform.SetParent(homeParent);
        gameObject.SetActive (true);
        transform.position = homePosition;
        transform.localScale = homeScale;
        transform.rotation = homeRotation;
    }

}

