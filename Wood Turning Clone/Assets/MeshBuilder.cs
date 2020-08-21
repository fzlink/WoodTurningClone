using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MeshBuilder
{
    public List<Vector3> Vertices { get; } = new List<Vector3>();
    public List<Vector3> Normals { get; } = new List<Vector3>();
    public List<Vector2> UVs { get; } = new List<Vector2>();
    private List<int> indices = new List<int>();

    public void AddTriangle(int ind1, int ind2, int ind3)
    {
        indices.Add(ind1);
        indices.Add(ind2);
        indices.Add(ind3);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = indices.ToArray();

        if (Normals.Count == Vertices.Count)
            mesh.normals = Normals.ToArray();

        if (UVs.Count == Vertices.Count)
            mesh.uv = UVs.ToArray();

        mesh.RecalculateBounds();
        return mesh;
    }
}

