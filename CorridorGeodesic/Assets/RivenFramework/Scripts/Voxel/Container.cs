//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.Voxel
{
    
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Container : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector3 containerPosition;
    public Dictionary<Vector3, Voxel> data;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private MeshData meshData = new MeshData();


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Initialize(Material _mat, Vector3 _pos)
    {
        InitializeReferences();
        data = new Dictionary<Vector3, Voxel>();
        meshRenderer.sharedMaterial = _mat;
        containerPosition = _pos;
    }

    private void InitializeReferences()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void ClearData()
    {
        data.Clear();
    }
    
    public void GenerateMesh()
    {
        meshData.ClearData();
        
        Vector3 blockPos;
        Voxel block;

        int counter = 0;
        Vector3[] faceVertices = new Vector3[4];
        Vector2[] faceUVs = new Vector2[4];

        foreach (var keyPair in data)
        {
            if (keyPair.Value.ID == 0) continue;
            
            blockPos = keyPair.Key;
            block = keyPair.Value;
            
            // Iterate over each face direction of the cube (6 times)
            for (int i = 0; i < 6; i++)
            {
                if (this[blockPos+voxelFaceChecks[i]].isSolid) continue;
                
                // Draw the face
                // Collect the appropriate vertices from the default vertices and add the block position
                for (int ii = 0; ii < 4; ii++)
                {
                    faceVertices[ii] = voxelVertices[vocelVertexIndex[i, ii]] + blockPos;
                    faceUVs[ii] = voxelUVs[ii];
                }

                for (int ii = 0; ii < 6; ii++)
                {
                    meshData.vertices.Add(faceVertices[voxelTris[i, ii]]);
                    meshData.UVs.Add(faceUVs[voxelTris[i, ii]]);
                    
                    meshData.triangles.Add(counter++);
                }
            }
        }
    }

    public void UploadMesh()
    {
        meshData.UploadMesh();
        
        if (meshRenderer == null) InitializeReferences();

        meshFilter.mesh = meshData.mesh;
        if (meshData.vertices.Count > 3)
        {
            meshCollider.sharedMesh = meshData.mesh;
        }

    }

    public Voxel this[Vector3 index]
    {
        get
        {
            if (data.ContainsKey(index))
            {
                return data[index];
            }
            else
            {
                return emptyVoxel;
            }
        }
        set
        {
            if (data.ContainsKey(index))
            {
                data[index] = value;
            }
            else
            {
                data.Add(index, value);
            }
        }
    }
    
    public static Voxel emptyVoxel = new Voxel() { ID = 0 };
    
    # region Mesh Data
    public struct MeshData
    {
        public Mesh mesh;
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector2> UVs;

        public bool initialized;

        public void ClearData()
        {
            if (!initialized)
            {
                vertices = new List<Vector3>();
                triangles = new List<int>();
                UVs = new List<Vector2>();

                initialized = true;
                mesh = new Mesh();
            }
            else
            {
                vertices.Clear();
                triangles.Clear();
                UVs.Clear();
                mesh.Clear();
            }
        }

        public void UploadMesh(bool _sharedVertices = false)
        {
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles,0,false);
            mesh.SetUVs(0, UVs);
            
            mesh.Optimize();
            
            mesh.RecalculateNormals();
            
            mesh.RecalculateBounds();
            
            mesh.UploadMeshData(false);
        }
    }
    # endregion

    #region Voxel Statics

    private static readonly Vector3[] voxelVertices = new Vector3[8]
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),

        new Vector3(0, 0, 1),
        new Vector3(1, 0, 1),
        new Vector3(0, 1, 1),
        new Vector3(1, 1, 1),
    };

    private static readonly Vector3[] voxelFaceChecks = new Vector3[6]
    {
        new Vector3(0, 0, -1),
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 1, 0),
    };

    private static readonly int[,] vocelVertexIndex = new int[6, 4]
    {
        { 0, 1, 2, 3 },
        { 4, 5, 6, 7 },
        { 4, 0, 6, 2 },
        { 5, 1, 7, 3 },
        { 0, 1, 4, 5 },
        { 2, 3, 6, 7 },
    };

    private static readonly Vector2[] voxelUVs = new Vector2[4]
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(1, 1),
    };

    private static readonly int[,] voxelTris = new int[6, 6]
    {
        { 0, 2, 3, 0, 3, 1 },
        { 0, 1, 2, 1, 3, 2 },
        { 0, 2, 3, 0, 3, 1 },
        { 0, 1, 2, 1, 3, 2 },
        { 0, 1, 2, 1, 3, 2 },
        { 0, 2, 3, 0, 3, 1 },
    };


    #endregion
}
}
