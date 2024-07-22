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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent (typeof (BzSliceableObject))]
public class MeshSlicer : MonoBehaviour
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
    private bool edgeSet = false;
    private Vector3 edgeVertex = Vector3.zero;
    private Vector2 edgeUV = Vector2.zero;
    private Plane edgePlane = new Plane ();
    private bool destroyOriginal;
    private List<Vector3> cutNormals = new List<Vector3> ();
    private List<float> inPointDistances = new List<float> ();
    private BzSliceableObject sliceableObject;
    private IBzMeshSlicer meshSlicer;
    private Vector3 homePosition;
    private GameObject backup;


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

    public async void ApplyCuts ()
    {
        MeshSlicer backup = Instantiate (this, transform.position, transform.rotation);
        backup.gameObject.name = name + "Backup";
        backup.gameObject.SetActive (false);
        backup.isCut = false;
        Item_Geodesic_Utility_GeoFolder.backupMeshes.Add (backup);

        bool sliced = false;

        //Slice the object
        var result = await sliceableObject.SliceAsync (Item_Geodesic_Utility_GeoFolder.plane1, meshSlicer);
        if (result.sliced)
        {
            sliced = true;
            foreach (var obj in result.resultObjects)
            {
                obj.gameObject.GetComponent<MeshSlicer>().isCut = true;
                Item_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                if (obj.side)
                {
                    //Slice the new object on the positive side of the cut, this time with the other plane
                    IBzMeshSlicer objSlicer = obj.gameObject.GetComponent<IBzMeshSlicer> ();
                    var result2 = await objSlicer.SliceAsync (Item_Geodesic_Utility_GeoFolder.plane2);
                    if (result2.sliced)
                    {
                        sliced = true;
                        //add the positive sides to the null list
                        foreach (var obj2 in result2.resultObjects)
                        {
                            obj.gameObject.GetComponent<MeshSlicer> ().isCut = true;
                            Item_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                            if (obj2.side)
                            {
                                Item_Geodesic_Utility_GeoFolder.nullSlices.Add (obj2.gameObject);
                            }
                            else
                            {
                                obj2.gameObject.transform.SetParent (Item_Geodesic_Utility_GeoFolder.plane2Meshes.transform, true);
                            }
                        }
                    }
                    else
                    {
                        //if slice 2 failed, we still add this object
                        Item_Geodesic_Utility_GeoFolder.nullSlices.Add (obj.gameObject);
                    }
                }
            }
        }

        else //if we didn't slice, we still try slicing with plane2
        {
            var result2 = await sliceableObject.SliceAsync (Item_Geodesic_Utility_GeoFolder.plane2, meshSlicer);
            if (result2.sliced)
            {
                sliced = true;
                foreach (var obj in result2.resultObjects)
                {
                    obj.gameObject.GetComponent<MeshSlicer> ().isCut = true;
                    Item_Geodesic_Utility_GeoFolder.slicedMeshes.Add (obj.gameObject);
                    if (obj.side)
                    {
                        Item_Geodesic_Utility_GeoFolder.nullSlices.Add (obj.gameObject);
                    }
                    else
                    {
                        obj.gameObject.transform.SetParent (Item_Geodesic_Utility_GeoFolder.plane2Meshes.transform, true);
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
                Vector3 worldPoint = transform.TransformPoint(testPoint);
                if (Item_Geodesic_Utility_GeoFolder.plane1.GetDistanceToPoint (worldPoint) < 0)
                {
                    Destroy (backup.gameObject); //If we're not even moving of course we don't need this
                    return;
                } else
                {
                    if (Item_Geodesic_Utility_GeoFolder.plane2.GetDistanceToPoint (worldPoint) < 0)
                    {
                        transform.SetParent (Item_Geodesic_Utility_GeoFolder.plane2Meshes.transform);
                        return;
                    }
                    else
                    {
                        Item_Geodesic_Utility_GeoFolder.nullSlices.Add (gameObject);
                        return;
                    }
                }
            }
        }


    }

}

