//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Keeps references to slices of an object when the object is sliced.
// Notes:
//
//=============================================================================

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh_Slicable))]
public class SlicedPartsReference : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private List<Mesh_Slicable> sliceList = new List<Mesh_Slicable>();
    private List<Mesh_Slicable> originalMesh = new List<Mesh_Slicable>();

    //=-----------------=
    // Reference Variables
    //=-----------------=

    /// <summary>
    /// Returns the list of sliced mesh parts, unless there are none in which case return the original mesh.
    /// </summary>
    public List<Mesh_Slicable> GetMeshes { get {
            if (sliceList.Count > 0)
            {
                return sliceList;
            }
            return originalMesh;
} }

    //public List<Mesh_Slicable> SlicedMeshesOnly => sliceList;
    //public List<Mesh_Slicable> originalMeshOnly => originalMesh;

    //=-----------------=
    // Mono Functions
    //=-----------------=

    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=

    public void Setup (Mesh_Slicable slicable)
    {
        originalMesh.Add (slicable);
    }

    public void DoReset ()
    {
        sliceList.Clear ();
    }

    public void AddSlice (Mesh_Slicable slice)
    {
        sliceList.Add (slice);
    }

}