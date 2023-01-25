using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] vertexColors;
    [SerializeField] Gradient gradient;
    [SerializeField] int xSize, zSize;
    float maxHeight = float.MinValue, minHeight = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        GenerateVertices();
        GenerateVertexColors();
        GenerateTriangles();
    }

    private void GenerateVertices()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;

                maxHeight = maxHeight > y ? maxHeight : y;
                minHeight = minHeight < y ? minHeight : y;

                vertices[i++] = new Vector3(x, y, z);
            }
        }
    }

    void GenerateVertexColors()
    {
        // We are assigning a color to each vertex in the mesh
        vertexColors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            // Normalize the height i.e the value must be between 0 and 1
            float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
            vertexColors[i] = gradient.Evaluate(height);
        }
    }

    void GenerateTriangles()
    {
        // We require 6 vertices to generate a single quad i.e 2 triangles and each triangle required 3 points
        // There are xSize * zSize number of quads
        triangles = new int[xSize * zSize * 6];

        int i = 0, vertexIndex = 0;
        for (int z = 0; z < zSize; z++)
        {
            // Every iteration generate a single quad composed of 2 triangles i.e 6 points of vertices
            for (int x = 0; x < xSize; x++)
            {
                // First triangle in the current quad
                triangles[i + 0] = vertexIndex;
                triangles[i + 1] = vertexIndex + xSize + 1;
                triangles[i + 2] = vertexIndex + 1;

                // Second triangle in the current quad
                triangles[i + 3] = vertexIndex + 1;
                triangles[i + 4] = vertexIndex + xSize + 1;
                triangles[i + 5] = vertexIndex + xSize + 2;

                vertexIndex++;
                i += 6;
            }
            vertexIndex++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.colors = vertexColors;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Mesh collider :
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
            // Draw a gizmo sphere of radius 0.1 at every vertex 
            Gizmos.DrawSphere(vertices[i], 0.1f);
    }
}
