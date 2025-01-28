//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Neverway.Framework.Voxel
{
public class Manger : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Material voxelRenderMaterial;
    public Vector3 voxelContainerBounds;
    public Vector3 voxelContainerBoundOffset;
    

    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Container voxelBounds;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        GameObject container = new GameObject("VoxelContainer");
        container.transform.parent = transform;
        voxelBounds = container.AddComponent<Container>();
        voxelBounds.Initialize(voxelRenderMaterial, Vector3.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position+voxelContainerBoundOffset, voxelContainerBounds);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    [ContextMenu("ForceClearVoxelSpace")]
    public void ForceClearVoxelSpace()
    {
        // Calculate boundary positions
        var boundsx = (voxelContainerBounds.x / 2) + (transform.position.x + voxelContainerBoundOffset.x);
        var boundsy = (voxelContainerBounds.y / 2) + (transform.position.y + voxelContainerBoundOffset.y);
        var boundsz = (voxelContainerBounds.z / 2) + (transform.position.z + voxelContainerBoundOffset.z);
        
        for (float x = -boundsx; x < boundsx; x++)
        {
            for (float z = -boundsz; z < boundsz; z++)
            {
                for (float y = -boundsy; y < boundsy; y++)
                {
                    voxelBounds[new Vector3(x, y, z)] = new Voxel() { ID = 0 };
                }
            }
        }
        
        voxelBounds.GenerateMesh();
        voxelBounds.UploadMesh();
    }
    
    [ContextMenu("ForceFillVoxelSpace")]
    public void ForceFillVoxelSpace()
    {
        // Calculate boundary positions
        var boundsx = Mathf.RoundToInt((voxelContainerBounds.x / 2) );
        var boundsy = Mathf.RoundToInt((voxelContainerBounds.y / 2) );
        var boundsz = Mathf.RoundToInt((voxelContainerBounds.z / 2) );
        
        print($"{boundsx}, {boundsy}, {boundsz}");
        
        for (int x = -boundsx; x < boundsx; x++)
        {
            for (int z = -boundsz; z < boundsz; z++)
            {
                for (int y = -boundsy; y < boundsy; y++)
                {
                    voxelBounds[new Vector3(x, y, z)] = new Voxel() { ID = 1 };
                }
            }
        }
        
        voxelBounds.GenerateMesh();
        voxelBounds.UploadMesh();
    }
}
}
