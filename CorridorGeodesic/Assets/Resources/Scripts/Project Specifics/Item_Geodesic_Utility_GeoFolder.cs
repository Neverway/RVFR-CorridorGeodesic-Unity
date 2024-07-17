//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Geodesic_Utility_GeoFolder : Item_Geodesic_Utility
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int maxAmmo=2;
    public int currentAmmo=2;
    public bool allowNoLinearSlicing;
    public float convergenceSpeed=0.25f;
    

    //=-----------------=
    // Private Variables
    //=-----------------=
    public float collapsedDistance;
    public Vector3 collapsedDirection;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private GameObject vacuumProjectile;
    [SerializeField] private GameObject riftObject;
    [SerializeField] private float projectileForce;
    public List<GameObject> deployedInfinityMarkers = new List<GameObject>();
    public GameObject deployedRift;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void UsePrimary()
    {
        DeployInfinityMarker();
    }
    
    public override void UseSecondary()
    {
        ConvergeInfinityMarkers();
    }
    
    public override void ReleaseSecondary()
    {
        //StopCoroutine(PeriodiclyFoldGeometry());
        //UndoGeometricCuts();
        //ApplyGeometricCuts();
    }
    
    public override void UseSpecial()
    {
        RecallInfinityMarkers();
        UndoGeometricCuts();
        collapsedDistance = 0;
        collapsedDirection = new Vector3();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void RecallInfinityMarkers()
    {
        var projectiles = FindObjectsOfType<VacuumProjectile>();
        foreach (var projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        if (deployedRift)
        {
            Destroy(deployedRift);
        }
        currentAmmo = maxAmmo;
        deployedInfinityMarkers.Clear();
    }
    private void DeployInfinityMarker()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;
        var projectile = Instantiate(vacuumProjectile, barrelTransform.transform.position, barrelTransform.transform.rotation, null);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * -projectileForce, ForceMode.Impulse);
        projectile.GetComponent<VacuumProjectile>().geoFolder = this; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
        deployedInfinityMarkers.Add(projectile);
    }

    private void ConvergeInfinityMarkers()
    {
        if (deployedInfinityMarkers.Count >= 2)
        {
            if (!deployedRift)
            {
                Vector3 midPoint = (deployedInfinityMarkers[0].transform.position + deployedInfinityMarkers[1].transform.position) * 0.5f;
                deployedRift = Instantiate(riftObject, midPoint, new Quaternion());
                deployedRift.transform.LookAt(deployedInfinityMarkers[0].transform);
                if (allowNoLinearSlicing) deployedRift.transform.rotation = new Quaternion(deployedRift.transform.rotation.x, deployedRift.transform.rotation.y, deployedRift.transform.rotation.z, deployedRift.transform.rotation.w);
                else deployedRift.transform.rotation = new Quaternion(0, deployedRift.transform.rotation.y, 0, deployedRift.transform.rotation.w);
                deployedRift.GetComponent<Rift>().distanceToMarker = Vector3.Distance(deployedInfinityMarkers[0].transform.position, deployedInfinityMarkers[1].transform.position) * 0.5f;
            }
            else
            {
                deployedRift.GetComponent<Rift>().DivergePortals(1);
                StartCoroutine(PeriodiclyFoldGeometry());
                //deployedRift.SetActive(true);
            }
        }
    }

    private IEnumerator PeriodiclyFoldGeometry()
    {
        yield return new WaitForSeconds(0f);
        UndoGeometricCuts();
        ApplyGeometricCuts();
    }

    private void ApplyGeometricCuts()
    {
        if (!deployedRift) return;
        collapsedDistance = 0;
        collapsedDirection = new Vector3();
        // Find all slice-able meshes
        foreach (var sliceableMesh in FindObjectsByType<MeshSlicer>(FindObjectsSortMode.None))
        {
            // Avoid cutting segments that are already cut
            if (!sliceableMesh.isCascadeSegment)
            {
                sliceableMesh.cutPlanes.Clear();
                sliceableMesh.cutPlanes.Add(deployedRift.GetComponent<Rift>().visualPlaneA);
                sliceableMesh.cutPlanes.Add(deployedRift.GetComponent<Rift>().visualPlaneB);
                sliceableMesh.ApplyCuts();
                
                // Collapse the space between the planes
                // Float
                collapsedDistance = Vector3.Distance(deployedRift.GetComponent<Rift>().visualPlaneA.transform.position, deployedRift.GetComponent<Rift>().visualPlaneB.transform.position);
                // Direction
                collapsedDirection = deployedRift.GetComponent<Rift>().visualPlaneA.gameObject.transform.up;

                //deployedRift.SetActive(false);
            }
        }

        // Delay the movement until after the cut has been fully created
        foreach (var sliceableMesh in FindObjectsByType<MeshSlicer>(FindObjectsSortMode.None))
        {
            if (sliceableMesh.segmentId == 2)
            {
                sliceableMesh.gameObject.transform.position += (collapsedDirection*collapsedDistance)/2;// We divide by two, so we can get half of the distance
            }
            else if (sliceableMesh.segmentId == 0)
            {
                sliceableMesh.gameObject.transform.position -= (collapsedDirection*collapsedDistance)/2;// We divide by two, so we can get half of the distance
            }
            // Any segment with the id of 1 is the result of some wierd artifacting, so we BURN IT IN A FIRELY BLAZE OF GLORY
            else if (sliceableMesh.segmentId == 1)
            {
                Destroy(sliceableMesh.gameObject);
            }
        }

        OffsetActorsAccordingToConvergenceDistance(_collapsing:true);
    }

    private void UndoGeometricCuts()
    {
        // Find all slice-able meshes
        foreach (var sliceableMesh in FindObjectsByType<MeshSlicer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (sliceableMesh.isCascadeSegment)
            {
                Destroy(sliceableMesh.gameObject);
            }
            else
            {
                sliceableMesh.gameObject.SetActive(true);
            }
        }
        
        OffsetActorsAccordingToConvergenceDistance(false);
    }

    private void OffsetActorsAccordingToConvergenceDistance(bool _collapsing)
    {
        if (!deployedRift) return;
        // Reposition objects in A & B space to match the change in distance between the markers
        // We divide by two, so we can get half of the distance
        var convergeDistance = (collapsedDirection * collapsedDistance) / 2;
        
        if (_collapsing)
        {
            // Push actors closer together
            deployedRift.GetComponent<Rift>().GetObjectSpacialLists();
            foreach (var _actor in deployedRift.GetComponent<Rift>().objectsInASpace)
            {
                _actor.gameObject.transform.position += convergeDistance;
            }
            foreach (var _actor in deployedRift.GetComponent<Rift>().objectsInBSpace)
            {
                _actor.gameObject.transform.position -= convergeDistance;
            }
        }
        else
        {
            // Pull actors further apart
            deployedRift.GetComponent<Rift>().GetObjectSpacialLists();
            foreach (var _actor in deployedRift.GetComponent<Rift>().objectsInASpace)
            {
                _actor.gameObject.transform.position -= convergeDistance;
            }
            foreach (var _actor in deployedRift.GetComponent<Rift>().objectsInBSpace)
            {
                _actor.gameObject.transform.position += convergeDistance;
            }
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
