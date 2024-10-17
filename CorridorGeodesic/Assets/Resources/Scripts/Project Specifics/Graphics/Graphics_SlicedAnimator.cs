//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_SlicedAnimator : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=

    //=-----------------=
    // Private Variables
    //=-----------------=

    [SerializeField] private Animator animatorA;
    [SerializeField] private Animator animatorB;
    [SerializeField] private Animator animatorNull;
    [SerializeField] private SkinnedMeshRenderer rendererA;
    [SerializeField] private SkinnedMeshRenderer rendererB;
    [SerializeField] private SkinnedMeshRenderer rendererNull;
    [SerializeField] private Material[] materials;
    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnRiftCreated += OnRiftCreated;
        Alt_Item_Geodesic_Utility_GeoGun.OnStateChanged += OnRiftChanged;
        SetUsePlanes (false);
    }

    private void OnDestroy ()
    {
        Alt_Item_Geodesic_Utility_GeoGun.OnRiftCreated -= OnRiftCreated;
    }

    private void Update()
    {
        if (Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.None && Alt_Item_Geodesic_Utility_GeoGun.currentState != RiftState.Preview)
        {
            SetAnimatorPlanes ();
        }

    }

    //=-----------------=
    // Internal Functions
    //=-----------------=

    private void OnRiftCreated ()
    {
        animatorB.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform);
        animatorNull.transform.SetParent (Alt_Item_Geodesic_Utility_GeoGun.deployedRift.transform);
        SetUsePlanes (true);
    }

    private void OnRiftChanged ()
    {
        SetAnimatorPlanes();
    }

    /// <summary>
    /// Set the cut planes for each renderer's materials.
    /// </summary>
    private void SetAnimatorPlanes ()
    {
        foreach (Material mat in rendererA.sharedMaterials)
        {
            mat.SetVector ("Slice Center One", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
            mat.SetVector ("Slice Center Two", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
            mat.SetVector ("Slice Normal One", Alt_Item_Geodesic_Utility_GeoGun.planeA.normal);
            mat.SetVector ("Slice Normal Two", Alt_Item_Geodesic_Utility_GeoGun.planeB.normal);
        }
        foreach (Material mat in rendererB.sharedMaterials)
        {
            mat.SetVector ("Slice Center One", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
            mat.SetVector ("Slice Center Two", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
            mat.SetVector ("Slice Normal One", Alt_Item_Geodesic_Utility_GeoGun.planeA.normal);
            mat.SetVector ("Slice Normal Two", Alt_Item_Geodesic_Utility_GeoGun.planeB.normal);
        }
        foreach (Material mat in rendererNull.sharedMaterials)
        {
            mat.SetVector ("Slice Center One", Alt_Item_Geodesic_Utility_GeoGun.planeAPos);
            mat.SetVector ("Slice Center Two", Alt_Item_Geodesic_Utility_GeoGun.planeBPos);
            mat.SetVector ("Slice Normal One", Alt_Item_Geodesic_Utility_GeoGun.planeA.normal);
            mat.SetVector ("Slice Normal Two", Alt_Item_Geodesic_Utility_GeoGun.planeB.normal);
        }
    }

    private void SetUsePlanes (bool _usePlanes)
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat ("Use Slice", _usePlanes ? 1 : 0);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=

    public void SetBool (string _name, bool _isPowered)
    {
        animatorA.SetBool (_name, _isPowered);
        animatorB.SetBool (_name, _isPowered);
        animatorNull.SetBool (_name, _isPowered);
    }


}