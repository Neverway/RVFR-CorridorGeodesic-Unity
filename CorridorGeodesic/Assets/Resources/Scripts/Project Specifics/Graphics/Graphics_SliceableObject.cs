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

    [SerializeField] private Graphics_SliceableObject child2;
    [SerializeField] private Graphics_SliceableObject childNull;

    private List<Material> materials = new List<Material>();

    private bool useSlice = false;

    private Plane[] planes = new Plane[2];

    [SerializeField] private Animator animator;

    public SliceSpace space = SliceSpace.Plane1;

    int n = 0;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private IEnumerator Start()
    {
        yield return new WaitUntil (()=>Graphics_SliceableObjectManager.Instance != null);

        if (child2 && childNull)
        {
            child2.space = SliceSpace.Plane2;
            childNull.space = SliceSpace.Null;
            child2.gameObject.SetActive (false);
            childNull.gameObject.SetActive (false);
            Graphics_SliceableObjectManager.Instance.AddToList (this);
        }

        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            materials.Add(new Material(rend.sharedMaterials[i]));
        }

        rend.sharedMaterials = materials.ToArray();

        foreach (Material mat in rend.materials)
        {
            mat.SetFloat ("_UseSlice", 0);
        }

        useSlice = false;
    }
    private void Update()
    {
        if (useSlice && rend.materials.Length > 0)
        {
            SetMaterialPlanes ();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void SetMaterialPlanes ()
    {

        Debug.Log ("Setting " + name);
        switch (space)
        {

            case SliceSpace.Plane1:

                foreach (Material mat in rend.materials)
                {
                    mat.SetVector ("_SliceNormalOne", Alt_Item_Geodesic_Utility_GeoGun.planeA.normal * -1);
                    mat.SetVector ("_SliceNormalTwo", Alt_Item_Geodesic_Utility_GeoGun.planeB.normal);

                    mat.SetVector ("_SliceCenterOne", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
                    mat.SetVector ("_SliceCenterTwo", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
                }
                break;
            case SliceSpace.Plane2:
                foreach (Material mat in rend.materials)
                {
                    mat.SetVector ("_SliceNormalOne", Alt_Item_Geodesic_Utility_GeoGun.planeA.normal);
                    mat.SetVector ("_SliceNormalTwo", Alt_Item_Geodesic_Utility_GeoGun.planeB.normal * -1);

                    mat.SetVector ("_SliceCenterOne", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
                    mat.SetVector ("_SliceCenterTwo", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
                }
                break;
            case SliceSpace.Null:
                foreach (Material mat in rend.materials)
                {
                    mat.SetVector ("_SliceNormalOne", planes[0].normal);
                    mat.SetVector ("_SliceNormalTwo", planes[1].normal);

                    mat.SetVector ("_SliceCenterOne", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
                    mat.SetVector ("_SliceCenterTwo", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
                }
                break;
        }
                
    }


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

        if (rend.materials.Length == 0)
            return;

        foreach (Material mat in rend.materials)
        {
            Debug.Log ("Set slice true " + name);
            mat.SetFloat ("_UseSlice", 1);
        }

        planes[0] = Alt_Item_Geodesic_Utility_GeoGun.planeA;
        planes[1] = Alt_Item_Geodesic_Utility_GeoGun.planeB;

        //Activate child graphics objects,
        //and parent them to their respective transforms.
        if (child2)
        {
            child2.gameObject.SetActive(true);
            child2.Slice ();
            child2.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform);
        }
        if (childNull)
        {
            childNull.gameObject.SetActive(true);
            childNull.Slice ();
            childNull.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.deployedRift.transform);
        }
    }
    public void StopSlicing()
    {
        Debug.Log ("Stop Slicing " + name);

        useSlice = false;
        if (rend.sharedMaterials.Length == 0)
            return;

        foreach (Material mat in rend.sharedMaterials)
        {
            mat.SetFloat("_UseSlice", 0);
        }

        if (child2)
        {
            child2.transform.SetParent (null);
            child2.gameObject.SetActive (false);
        }
        if (childNull)
        {
            childNull.transform.SetParent (null);
            childNull.gameObject.SetActive(false);
        }
    }

    public void AnimatorSetBool (bool isPowered)
    {
        animator.SetBool ("Powered", isPowered);
    }
}