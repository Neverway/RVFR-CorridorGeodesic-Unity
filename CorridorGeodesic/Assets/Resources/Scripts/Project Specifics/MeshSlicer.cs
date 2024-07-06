//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
// Source: https://www.youtube.com/watch?v=VwGiwDLQ40A
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDestroy : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int CutCascades = 1;
    public float ExplodeForce = 0;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool edgeSet = false;
    private Vector3 edgeVertex = Vector3.zero;
    private Vector2 edgeUV = Vector2.zero;
    private Plane edgePlane = new Plane();
    [SerializeField] private Transform posA, posB, posC;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DestroyMesh();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void DestroyMesh()
    {
        // Get the original mesh from the MeshFilter component
        var originalMesh = GetComponent<MeshFilter>().mesh;
        originalMesh.RecalculateBounds();  // Recalculate the mesh bounds

        // Create lists to store parts and sub-parts of the mesh
        var parts = new List<PartMesh>();
        var subParts = new List<PartMesh>();

        // Create the main part of the mesh
        var mainPart = new PartMesh()
        {
            UV = originalMesh.uv,
            Vertices = originalMesh.vertices,
            Normals = originalMesh.normals,
            Triangles = new int[originalMesh.subMeshCount][],
            Bounds = originalMesh.bounds
        };

        // Copy the triangles for each sub-mesh
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            mainPart.Triangles[i] = originalMesh.GetTriangles(i);
        }

        // Add the main part to the list of parts
        parts.Add(mainPart);

        // Iterate through the number of cut cascades
        for (var c = 0; c < CutCascades; c++)
        {
            // For each part, generate two new sub-parts using a random plane
            for (var i = 0; i < parts.Count; i++)
            {
                var bounds = parts[i].Bounds;
                bounds.Expand(0.5f);  // Expand the bounds slightly

                // Create a random plane within the bounds
                
                var plane = new Plane(UnityEngine.Random.onUnitSphere,new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z)));
                

                // Generate the two sub-parts
                subParts.Add(GenerateMesh(parts[i], plane, true));
                subParts.Add(GenerateMesh(parts[i], plane, false));
            }

            // Replace the parts list with the new sub-parts and clear subParts list
            parts = new List<PartMesh>(subParts);
            subParts.Clear();
        }

        // Create GameObjects for each part and add explosion force
        for (var i = 0; i < parts.Count; i++)
        {
            parts[i].MakeGameObject(this);
            // Uncomment the line below to add explosion force to the parts
            // parts[i].GameObject.GetComponent<Rigidbody>().AddForceAtPosition(parts[i].Bounds.center * ExplodeForce, transform.position);
        }

        // Destroy the original game object
        Destroy(gameObject);
    }

    
    private PartMesh GenerateMesh(PartMesh original, Plane plane, bool left)
    {
        // Create a new PartMesh instance to store the generated mesh
        var partMesh = new PartMesh() { };
        var ray1 = new Ray();
        var ray2 = new Ray();

        // Loop through all triangles in the original mesh
        for (var i = 0; i < original.Triangles.Length; i++)
        {
            var triangles = original.Triangles[i];
            edgeSet = false;

            // Loop through vertices in sets of 3 (each set represents a triangle)
            for (var j = 0; j < triangles.Length; j = j + 3)
            {
                // Determine on which side of the plane each vertex of the triangle lies
                var sideA = plane.GetSide(original.Vertices[triangles[j]]) == left;
                var sideB = plane.GetSide(original.Vertices[triangles[j + 1]]) == left;
                var sideC = plane.GetSide(original.Vertices[triangles[j + 2]]) == left;

                // Count how many vertices are on the specified side
                var sideCount = (sideA ? 1 : 0) +
                                (sideB ? 1 : 0) +
                                (sideC ? 1 : 0);

                // If no vertices are on the specified side, skip this triangle
                if (sideCount == 0)
                {
                    continue;
                }

                // If all vertices are on the specified side, add the whole triangle to the partMesh
                if (sideCount == 3)
                {
                    partMesh.AddTriangle(i,
                                         original.Vertices[triangles[j]], original.Vertices[triangles[j + 1]], original.Vertices[triangles[j + 2]],
                                         original.Normals[triangles[j]], original.Normals[triangles[j + 1]], original.Normals[triangles[j + 2]],
                                         original.UV[triangles[j]], original.UV[triangles[j + 1]], original.UV[triangles[j + 2]]);
                    continue;
                }

                // Identify the single vertex index that is on the opposite side
                var singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;

                // Calculate the intersection points of the plane with the edges of the triangle
                ray1.origin = original.Vertices[triangles[j + singleIndex]];
                var dir1 = original.Vertices[triangles[j + ((singleIndex + 1) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
                plane.Raycast(ray1, out var enter1);
                var lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.Vertices[triangles[j + singleIndex]];
                var dir2 = original.Vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray2.direction = dir2;
                plane.Raycast(ray2, out var enter2);
                var lerp2 = enter2 / dir2.magnitude;

                // Add the intersecting edges to the partMesh
                AddEdge(i,
                        partMesh,
                        left ? plane.normal * -1f : plane.normal,
                        ray1.origin + ray1.direction.normalized * enter1,
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                // If only one vertex is on the specified side, create a new triangle using the intersection points
                if (sideCount == 1)
                {
                    partMesh.AddTriangle(i,
                                        original.Vertices[triangles[j + singleIndex]],
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        original.Normals[triangles[j + singleIndex]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        original.UV[triangles[j + singleIndex]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    
                    continue;
                }

                // If two vertices are on the specified side, create two new triangles using the intersection points
                if (sideCount == 2)
                {
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]]);
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UV[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UV[triangles[j + singleIndex]], original.UV[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    continue;
                }
            }
        }

        // Finalize the mesh arrays
        partMesh.FillArrays();

        return partMesh;
    }

     
    private void AddEdge(int subMesh, PartMesh partMesh, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        // Check if this is the first edge to be added
        if (!edgeSet)
        {
            // If it is the first edge, set the edge vertex and UV coordinates
            edgeSet = true;
            edgeVertex = vertex1;
            edgeUV = uv1;
        }
        else
        {
            // If it's not the first edge, create a plane using three points
            edgePlane.Set3Points(edgeVertex, vertex1, vertex2);

            // Add a triangle to the partMesh with vertices and UVs
            // Determine the correct orientation of the vertices based on the side of the plane
            partMesh.AddTriangle(subMesh,
                edgeVertex,
                edgePlane.GetSide(edgeVertex + normal) ? vertex1 : vertex2,
                edgePlane.GetSide(edgeVertex + normal) ? vertex2 : vertex1,
                normal, // Normal for all vertices is the same
                normal,
                normal,
                edgeUV, // UV coordinates for the vertices
                uv1,
                uv2);
        }
    }



    //=-----------------=
    // External Functions
    //=-----------------=
}

public class PartMesh
{
    // Lists to store the mesh data
    private List<Vector3> _Vertices = new List<Vector3>();
    private List<Vector3> _Normals = new List<Vector3>();
    private List<List<int>> _Triangles = new List<List<int>>();
    private List<Vector2> _UVs = new List<Vector2>();

    // Arrays to store the finalized mesh data
    public Vector3[] Vertices;
    public Vector3[] Normals;
    public int[][] Triangles;
    public Vector2[] UV;

    // GameObject to represent this mesh in the scene
    public GameObject GameObject;
    
    // Bounds to keep track of the mesh's boundaries
    public Bounds Bounds = new Bounds();

    // Default constructor
    public PartMesh() {}

    // Method to add a triangle to the mesh
    public void AddTriangle(int submesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        // Ensure the submesh list is large enough
        if (_Triangles.Count - 1 < submesh)
            _Triangles.Add(new List<int>());

        // Add vertices and their indices to the lists
        _Triangles[submesh].Add(_Vertices.Count);
        _Vertices.Add(vert1);
        _Triangles[submesh].Add(_Vertices.Count);
        _Vertices.Add(vert2);
        _Triangles[submesh].Add(_Vertices.Count);
        _Vertices.Add(vert3);
        _Normals.Add(normal1);
        _Normals.Add(normal2);
        _Normals.Add(normal3);
        _UVs.Add(uv1);
        _UVs.Add(uv2);
        _UVs.Add(uv3);

        // Update the bounds of the mesh
        Bounds.min = Vector3.Min(Bounds.min, vert1);
        Bounds.min = Vector3.Min(Bounds.min, vert2);
        Bounds.min = Vector3.Min(Bounds.min, vert3);
        Bounds.max = Vector3.Max(Bounds.max, vert1);
        Bounds.max = Vector3.Max(Bounds.max, vert2);
        Bounds.max = Vector3.Max(Bounds.max, vert3);
    }

    // Method to fill the arrays from the lists
    public void FillArrays()
    {
        Vertices = _Vertices.ToArray();
        Normals = _Normals.ToArray();
        UV = _UVs.ToArray();
        Triangles = new int[_Triangles.Count][];
        for (var i = 0; i < _Triangles.Count; i++)
            Triangles[i] = _Triangles[i].ToArray();
    }

    // Method to create a GameObject from the mesh data
    public void MakeGameObject(MeshDestroy original)
    {
        // Create a new GameObject and set its transform to match the original
        GameObject = new GameObject(original.name);
        GameObject.transform.position = original.transform.position;
        GameObject.transform.rotation = original.transform.rotation;
        GameObject.transform.localScale = original.transform.localScale;

        // Create a new Mesh and assign the vertex, normal, and UV data
        var mesh = new Mesh();
        mesh.name = original.GetComponent<MeshFilter>().mesh.name;

        mesh.vertices = Vertices;
        mesh.normals = Normals;
        mesh.uv = UV;
        for (var i = 0; i < Triangles.Length; i++)
            mesh.SetTriangles(Triangles[i], i, true);
        Bounds = mesh.bounds;

        // Add a MeshRenderer component and assign the materials from the original
        var renderer = GameObject.AddComponent<MeshRenderer>();
        renderer.materials = original.GetComponent<MeshRenderer>().materials;

        // Add a MeshFilter component and assign the generated mesh
        var filter = GameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;

        // Add a MeshCollider component and set it to convex
        var collider = GameObject.AddComponent<MeshCollider>();
        collider.convex = true;

        // Add a MeshDestroy component and copy the settings from the original
        var meshDestroy = GameObject.AddComponent<MeshDestroy>();
        meshDestroy.CutCascades = original.CutCascades;
        meshDestroy.ExplodeForce = original.ExplodeForce;
    }
}

