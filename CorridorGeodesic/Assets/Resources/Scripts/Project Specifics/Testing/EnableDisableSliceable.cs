using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnableDisableSliceable : MonoBehaviour
{
    public Mesh_Slicable slicableThatIsNotSelf;
    public GameObject whenThisObjectEnablesOrDisables;
    private bool lastActive;
    private MeshCollider colldier;
    private new MeshRenderer renderer;

    private List<MeshCollider> allPartsColliders;
    private List<MeshRenderer> allPartsRenderers;

    private static int count;

    public void Awake()
    {
        //Debug.LogError("THIS SCRIPT DOES NOT WORK");
        transform.SetParent(null);
        //Set to a unique name
        slicableThatIsNotSelf.gameObject.name += " : " + count++;

        allPartsColliders = new List<MeshCollider>();
        allPartsRenderers = new List<MeshRenderer>();

        //slicable = GetComponent<Mesh_Slicable>();
        renderer = slicableThatIsNotSelf.GetComponent<MeshRenderer>();
        colldier = slicableThatIsNotSelf.GetComponent<MeshCollider>();
    }
    public void Update()
    {
        if (lastActive != whenThisObjectEnablesOrDisables.activeInHierarchy)
        {
            lastActive = whenThisObjectEnablesOrDisables.activeInHierarchy;
            GetSlicedParts();
        }
        DelayedSetEnable(whenThisObjectEnablesOrDisables.activeInHierarchy);
    }

    private void DelayedSetEnable(bool isActive)
    {
        SetActiveForFullObject(isActive);
        SetActiveForSlicedParts(isActive);
    }

    private void GetSlicedParts()
    {
        //todo: THIS IS FUCKING AWFUL BUT I TRIED MY BEST
        ClearSlicedParts();
        foreach (GameObject slicable in Alt_Item_Geodesic_Utility_GeoGun.slicedMeshes)
        {
            if (slicable.name.StartsWith("[CUT] " + slicableThatIsNotSelf.gameObject.name))
            {
                allPartsColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                allPartsRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
            }
        }
        if (Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes != null)
        {
            foreach (Transform slicable in Alt_Item_Geodesic_Utility_GeoGun.planeBMeshes.transform)
            {
                if (slicable.name.StartsWith("[CUT] " + slicableThatIsNotSelf.gameObject.name))
                {
                    allPartsColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                    allPartsRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }
        if (Alt_Item_Geodesic_Utility_GeoGun.nullSlices != null)
        {
            foreach (GameObject slicable in Alt_Item_Geodesic_Utility_GeoGun.nullSlices)
            {
                if (slicable != null && slicable.name.StartsWith("[CUT] " + slicableThatIsNotSelf.gameObject.name))
                {
                    allPartsColliders.Add(slicable.gameObject.GetComponent<MeshCollider>());
                    allPartsRenderers.Add(slicable.gameObject.GetComponent<MeshRenderer>());
                }
            }
        }
    }
    private void ClearSlicedParts()
    {
        allPartsColliders.Clear();
        allPartsRenderers.Clear();
    }

    private void SetActiveForSlicedParts(bool active)
    {
        Debug.Log("YAYA setactive to " + allPartsColliders.Count + " colliders and " + allPartsRenderers + " renderers");
        foreach (MeshCollider c in allPartsColliders)
            c.enabled = active;

        foreach (MeshRenderer r in allPartsRenderers)
            r.enabled = active;
    }
    private void SetActiveForFullObject(bool active)
    {
        if (active)
            gameObject.SetActive(true);

        colldier.enabled = active;
        renderer.enabled = active;
    }
}
