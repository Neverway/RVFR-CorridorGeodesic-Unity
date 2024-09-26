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
public class Mesh_Slicable : MonoBehaviour
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
    [Tooltip("Optional field for object that slices can be sent to.")]
    [SerializeField] private SlicedPartsReference partsReference;
    private BzSliceableObject sliceableObject;
    private IBzMeshSlicer meshSlicer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        meshSlicer = GetComponent<IBzMeshSlicer> ();
        sliceableObject = GetComponent<BzSliceableObject>();
        if (partsReference) partsReference.Setup (this);

        if (!sliceableObject.defaultSliceMaterial)
        {
            sliceableObject.defaultSliceMaterial = CorGeo_ReferenceManager.Instance.nullSpace;
        }
    }
    
    public void Update()
    {
        foreach (var meshCollider in gameObject.GetComponents<MeshCollider>())  //todo no no no no no
        {
            if (!meshCollider.isTrigger)
            {
                meshCollider.convex = false;
            }
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
        if (gameObject.name.StartsWith("[CUT]"))
        {
            Debug.LogError("TRYING TO CUT " + gameObject.name + " BUT ITS ALREADY CUT: Maybe make sure read/write is enabled for this model?");
            return;
        }

        if (partsReference != null)
        {
            partsReference.DoReset ();
        }
        Mesh_Slicable sliceThis = Instantiate (this, transform.position, transform.rotation);
        sliceThis.gameObject.name = $"[CUT] {name}";
        isCut = false;
        Alt_Item_Geodesic_Utility_GeoGun.originalSliceableObjects.Add(this);
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
        var result = await sliceableObject.SliceAsync (Alt_Item_Geodesic_Utility_GeoGun.planeA, meshSlicer);
        if (result.sliced)
        {

            _original.SetActive (false);
            sliced = true;
            foreach (var obj in result.resultObjects)
            {
                Mesh_Slicable objMesh1 = GetComponent<Mesh_Slicable> ();
                objMesh1.isCut = true;
                foreach (Collider collider in obj.gameObject.GetComponents<Collider> ())
                {
                    collider.isTrigger = isTrigger;
                    MeshCollider meshColl = collider as MeshCollider;
                    if (meshColl != null) //todo is this a bug? i think i meant to only do this if the mesh was a trigger
                    {
                        meshColl.convex = true;
                    }
                }
                
                Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes.Add (obj.gameObject);
                if (obj.side)
                {
                    //Slice the new object on the positive side of the cut, this time with the other plane
                    IBzMeshSlicer objSlicer = obj.gameObject.GetComponent<IBzMeshSlicer> ();
                    var result2 = await objSlicer.SliceAsync (Alt_Item_Geodesic_Utility_GeoGun.planeB);
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
                                MeshCollider meshColl = coll as MeshCollider;
                                if (meshColl != null)
                                {
                                    meshColl.convex = true;
                                }
                            }
                            Mesh_Slicable objMesh2 = obj2.gameObject.GetComponent<Mesh_Slicable> ();
                            objMesh2.isCut = true;
                            Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes.Add (obj.gameObject);

                            if (partsReference)
                            {
                                partsReference.AddSlice (objMesh2);
                            }

                            if (obj2.side)
                            {
                                Alt_Item_Geodesic_Utility_GeoGun.nullSlices.Add (obj2.gameObject);
                            }
                            else
                            {
                                obj2.gameObject.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform);
                            }
                        }
                    }
                    else
                    {
                        //if slice 2 failed, we still add this object
                        Alt_Item_Geodesic_Utility_GeoGun.nullSlices.Add (obj.gameObject);
                        if (partsReference)
                        {
                            partsReference.AddSlice (obj.gameObject.GetComponent<Mesh_Slicable> ());
                        }
                    }
                }
                else //if !obj.side
                {
                    if (partsReference)
                    {
                        partsReference.AddSlice (obj.gameObject.GetComponent<Mesh_Slicable> ());
                    }
                }
            }
        }

        else //if we didn't slice, we still try slicing with plane2
        {
            var result2 = await sliceableObject.SliceAsync (Alt_Item_Geodesic_Utility_GeoGun.planeB, meshSlicer);
            if (result2.sliced)
            {
                _original.SetActive (false);
                sliced = true;
                foreach (var obj3 in result2.resultObjects)
                {
                    foreach (Collider collider in obj3.gameObject.GetComponents<Collider> ())
                    {
                        collider.isTrigger = isTrigger;
                        MeshCollider meshColl = coll as MeshCollider;
                        if (meshColl != null)
                        {
                            meshColl.convex = true;
                        }
                    }
                    Mesh_Slicable objMesh3 = obj3.gameObject.GetComponent<Mesh_Slicable> ();
                    objMesh3.isCut = true;
                    Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes.Add (obj3.gameObject);
                    if (partsReference)
                    {
                        partsReference.AddSlice (objMesh3);
                    }

                    if (obj3.side)
                    {
                        Alt_Item_Geodesic_Utility_GeoGun.nullSlices.Add (obj3.gameObject);
                    }
                    else
                    {
                        obj3.gameObject.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform);
                    }
                }
            }
        }


        if (!sliced)
        {
            //If all the slices miss, we have to figure out where this mesh is.

            MeshFilter meshFilter = GetComponent<MeshFilter> ();
            if (meshFilter != null && meshFilter.mesh.vertices.Length > 0)
            {
                var vert = meshFilter.mesh.vertices[0];
                Vector3 testPoint = new Vector3 (vert.x, vert.y, vert.z);
                Vector3 worldPoint = transform.TransformPoint (testPoint);
                if (Alt_Item_Geodesic_Utility_GeoGun.planeA.GetDistanceToPoint (worldPoint) < 0)
                {
                    //Entire object was outside nullspace plane1
                    _original.gameObject.SetActive (true);
                    Destroy (gameObject);
                    return;
                }
                else
                {
                    if (Alt_Item_Geodesic_Utility_GeoGun.planeB.GetDistanceToPoint (worldPoint) < 0)
                    {
                        //entire object was outside nullspace plane2
                        _original.SetActive (true);
                        _original.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform);
                        Destroy (gameObject);
                        return;
                    }
                    else
                    {
                        //Entire object was in the nullspace
                        Alt_Item_Geodesic_Utility_GeoGun.nullSlices.Add (_original);
                        Destroy (gameObject);
                        return;
                    }
                }
            }
        }
    }

    public void GoHome()
    {
        if (partsReference != null)
        {
            partsReference.DoReset ();
        }
        transform.SetParent(homeParent);
        gameObject.SetActive (true);
        transform.position = homePosition;
        transform.localScale = homeScale;
        transform.rotation = homeRotation;
    }

}

