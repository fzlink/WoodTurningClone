using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FZTerrain : MonoBehaviour
{

    public float height = 1f;
    public float width = 1f;
    public float depth = 1f;
    public int segmentCount = 10;

    private void Start()
    {
        CreateTerrain();
    }

    private void CreateTerrain()
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        for (int i = 0; i <= segmentCount; i++)
        {
            float z = height * i;
            float v = (1f/ segmentCount) * i;

            for (int j = 0; j <= segmentCount; j++)
            {
                float x = width * j;
                float u = (1f / segmentCount) * j;

                Vector3 offset = new Vector3(x, UnityEngine.Random.Range(0, depth), z);
                Vector2 uv = new Vector2(u, v);
                bool buildTriangles = i > 0 && j > 0;
                CreateQuadForGrid(meshBuilder, offset, uv, buildTriangles,segmentCount + 1);
            }
        }
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null)
        {
            Mesh mesh = meshBuilder.CreateMesh();
            mesh.RecalculateNormals();
            filter.sharedMesh = mesh;
        }

    }

    private void CreateQuad(MeshBuilder meshBuilder, Vector3 offset)
    {
        meshBuilder.Vertices.Add(new Vector3(0, 0, 0) + offset);
        meshBuilder.Normals.Add(Vector3.up);
        meshBuilder.UVs.Add(new Vector2(0, 0));

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, height) + offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(width, 0.0f, height) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(width, 0.0f, 0.0f) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    private void CreateQuadForGrid(MeshBuilder meshBuilder,Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
    {
        meshBuilder.Vertices.Add(position);
        meshBuilder.UVs.Add(uv);

        if (buildTriangles)
        {
            int baseIndex = meshBuilder.Vertices.Count - 1;
            int ind0 = baseIndex;
            int ind1 = baseIndex - 1;
            int ind2 = baseIndex - vertsPerRow;
            int ind3 = baseIndex - vertsPerRow - 1;

            meshBuilder.AddTriangle(ind0, ind2, ind1);
            meshBuilder.AddTriangle(ind2, ind3, ind1);
        }
    }

  
}
