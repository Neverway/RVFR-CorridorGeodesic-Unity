//===================== (Neverway 2024) Written by Andre Blunt ================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_SliceableObject : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private Renderer rend;

    [SerializeField] private Graphics_SliceableObject childA;
    [SerializeField] private Graphics_SliceableObject childB;

    private List<Material> materials = new List<Material>();

    private bool useSlice;

    private Plane[] planes;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        if (!childA && !childB)
            Graphics_SliceableObjectManager.Instance.AddToList(this);

        planes[0] = Alt_Item_Geodesic_Utility_GeoGun.planeA;
        planes[1] = Alt_Item_Geodesic_Utility_GeoGun.planeB;

        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            materials.Add(new Material(rend.sharedMaterials[i]));
        }

        rend.sharedMaterials = materials.ToArray();
    }
    private void Update()
    {
        if (useSlice && rend.sharedMaterials.Length > 0)
        {
            foreach (Material mat in rend.sharedMaterials)
            {
                mat.SetVector("_SliceNormalOne", planes[0].normal * -1);
                mat.SetVector("_SliceNormalTwo", planes[1].normal);

                //mat.SetVector("_SliceCenterOne", );
                //mat.SetVector("_SliceCenterTwo", );
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void StartSlicing()
    {
        if (GeometryUtility.TestPlanesAABB(planes, rend.bounds))
            Slice();
    }
    private void Slice()
    {
        useSlice = true;
        if (rend.sharedMaterials.Length == 0)
            return;

        if (childA)
            childA.gameObject.SetActive(true);
        if (childB)
            childB.gameObject.SetActive(true);

        foreach (Material mat in rend.sharedMaterials)
        {
            mat.SetFloat("_UseSlice", 1);
        }
    }
    public void StopSlicing()
    {
        useSlice = false;
        if (rend.sharedMaterials.Length == 0)
            return;

        foreach (Material mat in rend.sharedMaterials)
        {
            mat.SetFloat("_UseSlice", 0);
        }

        if (childA)
            childA.gameObject.SetActive(false);
        if (childB)
            childB.gameObject.SetActive(false);
    }
}