using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FZCylinder : MonoBehaviour
{

    public int radialSegmentCount = 10;
    public int heightSegmentCount = 4;
    public float radius = 0.5f;
    public float height = 2f;

    private MeshFilter filter;
    public MeshCollider collider;
    public Mesh Mesh { get { return filter.mesh; } }

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
        collider = GetComponent<MeshCollider>();
        CreateCylinder();
    }

    private void CreateCylinder()
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        float heightInc = height / heightSegmentCount;

        for (int i = 0; i <= heightSegmentCount; i++)
        {
            Vector3 centerPos = Vector3.up * heightInc * i;

            float v = (float)i / heightSegmentCount;

            CreateRing(meshBuilder, radialSegmentCount, centerPos, radius, v, i > 0);
        }

        CreateCap(meshBuilder, Vector3.zero, true);
        CreateCap(meshBuilder, Vector3.up * height, false);
        Mesh mesh = meshBuilder.CreateMesh();
        if (filter != null)
            filter.sharedMesh = mesh;
        if (collider != null)
            collider.sharedMesh = mesh;
        SendMessage("MeshRegenerated");
    }

    private void CreateRing(MeshBuilder meshBuilder, int segmentCount, Vector3 center, float radius, float v, bool buildTriangles)
    {
        float angleInc = (Mathf.PI * 2.0f) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            meshBuilder.Vertices.Add(center + unitPosition * radius);
            meshBuilder.Normals.Add(unitPosition);
            meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));

            if (i > 0 && buildTriangles)
            {
                int baseIndex = meshBuilder.Vertices.Count - 1;

                int vertsPerRow = segmentCount + 1;

                int index0 = baseIndex;
                int index1 = baseIndex - 1;
                int index2 = baseIndex - vertsPerRow;
                int index3 = baseIndex - vertsPerRow - 1;

                meshBuilder.AddTriangle(index0, index2, index1);
                meshBuilder.AddTriangle(index2, index3, index1);
            }
        }
    }

    private void CreateRingBent(MeshBuilder meshBuilder, int segmentCount, Vector3 centre, float radius, float v, bool buildTriangles, Quaternion rotation)
    {
        float angleInc = (Mathf.PI * 2.0f) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            unitPosition = rotation * unitPosition;

            meshBuilder.Vertices.Add(centre + unitPosition * radius);
            meshBuilder.Normals.Add(unitPosition);
            meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));

            if (i > 0 && buildTriangles)
            {
                int baseIndex = meshBuilder.Vertices.Count - 1;

                int vertsPerRow = segmentCount + 1;

                int index0 = baseIndex;
                int index1 = baseIndex - 1;
                int index2 = baseIndex - vertsPerRow;
                int index3 = baseIndex - vertsPerRow - 1;

                meshBuilder.AddTriangle(index0, index2, index1);
                meshBuilder.AddTriangle(index2, index3, index1);
            }
        }
    }

    void CreateCap(MeshBuilder meshBuilder, Vector3 center, bool reverseDirection)
    {
        Vector3 normal = reverseDirection ? Vector3.down : Vector3.up;

        //one vertex in the center:
        meshBuilder.Vertices.Add(center);
        meshBuilder.Normals.Add(normal);
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));

        int centreVertexIndex = meshBuilder.Vertices.Count - 1;

        //vertices around the edge:
        float angleInc = (Mathf.PI * 2.0f) / radialSegmentCount;

        for (int i = 0; i <= radialSegmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            meshBuilder.Vertices.Add(center + unitPosition * radius);
            meshBuilder.Normals.Add(normal);

            Vector2 uv = new Vector2(unitPosition.x + 1.0f, unitPosition.z + 1.0f) * 0.5f;
            meshBuilder.UVs.Add(uv);

            //build a triangle:
            if (i > 0)
            {
                int baseIndex = meshBuilder.Vertices.Count - 1;

                if (reverseDirection)
                    meshBuilder.AddTriangle(centreVertexIndex, baseIndex - 1,
                        baseIndex);
                else
                    meshBuilder.AddTriangle(centreVertexIndex, baseIndex,
                        baseIndex - 1);
            }
        }
    }
}
