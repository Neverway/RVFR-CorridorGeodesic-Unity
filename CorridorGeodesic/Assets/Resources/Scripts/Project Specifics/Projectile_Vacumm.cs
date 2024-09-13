//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile_Vacumm : Projectile
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    //[SerializeField] private float castRadius;
    //[SerializeField] private Vector3 castOffset;
    [SerializeField] private float pinOffset;
    [SerializeField] private float gridSize = 1;
    public bool pinned => disabled;
    [SerializeField] private GameObject spawnOnDeath;

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [FormerlySerializedAs("geoFolder")] [HideInInspector] public Alt_Item_Geodesic_Utility_GeoGun geoGun; // Get a reference to the gun that spawned the projectile, so we know who to give ammo to on a lifetime expiration
    
    //=-----------------=
    // Mono Functions
    //=-----------------=
    public override void Awake()
    {
        base.Awake();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void OnCollision(RaycastHit hit)
    {
        base.OnCollision(hit);

        bool killScheduled = false;

        //If we hit an Outlet, then snap to it's attachpoint.
        if (hit.collider.gameObject.TryGetComponent<BulbOutlet> (out var outlet))
        {
            transform.position = outlet.attachPoint.position;
            transform.DORotateQuaternion (Quaternion.LookRotation (outlet.attachPoint.forward), 0.08f);
            Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.nixieTubePin);
            disabled = true;
            return;
        }

        hit.collider.gameObject.TryGetComponent<Mesh_Slicable>(out var _out);
        if (_out)
        {
            if (hit.collider is not MeshCollider) return;
            MeshCollider mCollider = (MeshCollider)hit.collider;

            Mesh colMesh = mCollider.sharedMesh;

            int triIndex = hit.triangleIndex;

            DisplayDebugTriangle(colMesh, triIndex, hit.collider.transform);

            if (hit.collider.gameObject.TryGetComponent(out Renderer rend))
            {
                int subMeshIndex = GetSubMeshIndex(colMesh, triIndex);
                if (subMeshIndex != -1 && !CorGeo_ReferenceManager.Instance.conductiveMats.Contains(rend.sharedMaterials[subMeshIndex]))
                {
                    KillProjectile();
                    killScheduled = true;
                }
            }
            if (killScheduled)
                return;

            // Set rotation to match face normal
            transform.DORotateQuaternion(Quaternion.LookRotation(-hit.normal),0.08f);
            
            // Snap to a grid along the face
            // Get the hit position and normal
            Vector3 hitPosition = hit.point + hit.normal * pinOffset;
            // Figure out the relative Vector3.direction from the hit.point
            Vector3 relativeRight, relativeUp;
            if (Mathf.Abs(hit.normal.y) > 0.99f) // Nearly vertical surface (e.g., floor or ceiling)
            {
                relativeRight = Vector3.right; // Use world X axis for snapping
                relativeUp = Vector3.forward;  // Use world Z axis for snapping
            }
            else
            {
                // Calculate relative axes based on the hit normal
                relativeRight = Vector3.Cross(hit.normal, Vector3.up);
                relativeUp = Vector3.Cross(hit.normal, relativeRight);
            }
            // Project the hit position onto these axes and snap along them
            hitPosition += relativeRight * (Mathf.Round(Vector3.Dot(hitPosition, relativeRight) / gridSize) * gridSize - Vector3.Dot(hitPosition, relativeRight));
            hitPosition += relativeUp * (Mathf.Round(Vector3.Dot(hitPosition, relativeUp) / gridSize) * gridSize - Vector3.Dot(hitPosition, relativeUp));
            // Update the marker's position to the snapped position
            transform.position = hitPosition;
            transform.localPosition += hit.normal * pinOffset;

            Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.nixieTubePin);
            disabled = true;
        }
        else
        {
            KillProjectile();
        }
    }

    public void KillProjectile (bool removeProjectileFromList = true)
    {
        // Note: I added removeProjectileFromList because otherwise, calling this function from the geoFolder, will
        // update the list while the utility is trying to iterate through it (That's no good!) ~Liz
        
        // Give the projectile back to the weapon
        if (geoGun)
        {
            if (geoGun.currentAmmo < geoGun.maxAmmo)
            {
                geoGun.currentAmmo++;
            }

            if (removeProjectileFromList) geoGun.deployedInfinityMarkers.Remove(this);
        }

        //Spawn particle effect
        if (spawnOnDeath)
        {
            Instantiate (spawnOnDeath, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
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
        if (!mesh.isReadable)
            return;
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
    public override void OnLifetimeOut()
    {
        base.OnLifetimeOut();
        KillProjectile();
    }
}