//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VacuumProjectile : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float castRadius;
    [SerializeField] private Vector3 castOffset;
    [SerializeField] private float pinOffset;
    [HideInInspector] public bool pinned;
    [SerializeField] private GameObject spawnOnDeath;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [HideInInspector] public GameObject geoFolder; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
    private Rigidbody objectRigidbody;
    private RaycastHit faceHit;
    [SerializeField] private LayerMask layerMask;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(Lifetime());
    }

    private void Update()
    {
        if (pinned) return;
        CollisionTest();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CollisionTest()
    {
        if (Physics.SphereCast(transform.position+castOffset, castRadius, transform.forward, out faceHit, 1f, layerMask))
        {
            bool killScheduled = false;
            if (faceHit.collider is MeshCollider)
            {
                MeshCollider mCollider = (MeshCollider)faceHit.collider;

                Mesh colMesh = mCollider.sharedMesh;

                int triIndex = faceHit.triangleIndex;

                DisplayDebugTriangle(colMesh, triIndex, faceHit.collider.transform);

                if (faceHit.collider.gameObject.TryGetComponent(out Renderer rend))
                {
                    int subMeshIndex = GetSubMeshIndex(colMesh, triIndex);
                    if(subMeshIndex != -1 && ReferenceManager.Instance.conductiveMats.Contains(rend.sharedMaterials[subMeshIndex]))
                    {
                        KillProjectile();
                        killScheduled = true;
                    }
                }
            }
            if (!killScheduled && faceHit.collider.gameObject.TryGetComponent<CorGeo_AntiProjectile> (out var component))
            {
                KillProjectile();
                killScheduled = true;
            }
            if (killScheduled)
                return;

            // Disable physics
            objectRigidbody.isKinematic = true;
            
            // Set rotation to match face normal
            transform.rotation = Quaternion.LookRotation(-faceHit.normal);
            transform.position = faceHit.point;
            transform.localPosition += faceHit.normal*pinOffset;
            
            pinned = true;
        }
    }
    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(1);
        if (pinned) yield break; // Exit if the lamp has landed 
        KillProjectile ();
    }

    public void KillProjectile (bool removeProjectileFromList = true)
    {
        // Note: I added removeProjectileFromList because otherwise, calling this function from the geoFolder, will
        // update the list while the utility is trying to iterate through it (That's no good!) ~Liz
        
        // Give the projectile back to the weapon
        if (geoFolder)
        {
            if (geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder> ())
            {
                if (geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder> ().currentAmmo < geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder> ().maxAmmo)
                {
                    geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder> ().currentAmmo++;
                }

                if (removeProjectileFromList) geoFolder.GetComponent<ALTItem_Geodesic_Utility_GeoFolder> ().deployedInfinityMarkers.Remove (gameObject);
            }
        }

        //Spawn particle effect
        if (spawnOnDeath)
        {
            Instantiate (spawnOnDeath, transform.position, Quaternion.identity);
        }
        
        // Erase the projectile
        Destroy (gameObject);
    }
    int GetSubMeshIndex(Mesh mesh, int triIndex)
    {
        int triangleCounter = 0;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            triangleCounter += mesh.GetSubMesh(i).indexCount / 3;
            if (triIndex < triangleCounter)
            {
                return i;
            }
        }
        return -1;
    }
    void DisplayDebugTriangle(Mesh mesh, int triangleIndex, Transform hitTransform)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triangleIndex * 3 + 2]];
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1, Color.red, 5);
        Debug.DrawLine(p1, p2, Color.red, 5);
        Debug.DrawLine(p2, p0, Color.red, 5);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
