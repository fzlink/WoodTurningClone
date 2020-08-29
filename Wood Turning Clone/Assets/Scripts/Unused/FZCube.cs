using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FZCube : MonoBehaviour
{

    public float height = 1f;
    public float width = 1f;
    public float length = 1f;

    void Start()
    {
        CreateCube();
    }

    private void CreateCube()
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        Vector3 upDir = Vector3.up * height;
        Vector3 rightDir = Vector3.right * width;
        Vector3 forwardDir = Vector3.forward * length;

        Vector3 nearCorner = Vector3.zero;
        Vector3 farCorner = upDir + rightDir + forwardDir;

        CreateQuad(meshBuilder, nearCorner, forwardDir, rightDir);
        CreateQuad(meshBuilder, nearCorner, rightDir, upDir);
        CreateQuad(meshBuilder, nearCorner, upDir, forwardDir);

        CreateQuad(meshBuilder, farCorner, -rightDir, -forwardDir);
        CreateQuad(meshBuilder, farCorner, -upDir, -rightDir);
        CreateQuad(meshBuilder, farCorner, -forwardDir, -upDir);

        Mesh mesh = meshBuilder.CreateMesh();
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null)
            filter.sharedMesh = mesh;

    }

    private void CreateQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
    {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(offset);
        meshBuilder.Normals.Add(normal);
        meshBuilder.UVs.Add(new Vector2(0, 0));

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }
}
