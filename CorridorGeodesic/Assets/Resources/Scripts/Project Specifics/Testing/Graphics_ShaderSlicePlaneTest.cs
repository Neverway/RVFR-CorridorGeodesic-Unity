//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_ShaderSlicePlaneTest: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private bool invertSlice;
    private List<Material> materials = new List<Material>();
    [SerializeField] private Renderer rend;
    [SerializeField] private Graphics_SlicePlane[] slicePlanes;

    //=-----------------=
    // Reference Variables
    //=-----------------=

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            materials.Add(new Material(rend.sharedMaterials[i]));
        }

        rend.sharedMaterials = materials.ToArray();
    }
    private void Update()
    {
        if (rend.sharedMaterials.Length == 0)
            return;

        foreach (var mat in rend.sharedMaterials)
        {
            mat.SetFloat("_UseSlice", 1);

            for (int i = 0; i < slicePlanes.Length; i++)
            {
                Graphics_SlicePlane plane = slicePlanes[i];
                mat.SetVector(plane.sliceNormalPropertyName, plane.transform.forward * (invertSlice ? -1 : 1));
                mat.SetVector(plane.sliceCenterPropertyName, plane.transform.position);
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
