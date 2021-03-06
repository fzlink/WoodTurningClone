﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeformableMesh : MonoBehaviour
{
    public float maximumDepression;
    public float deformSpeed;
    public float cutFullyThreshold;
    public Vector3[] originalVertices;
    public Vector3[] modifiedVertices;
    public Vector3[] normals;
    private FZCylinder cylinder;
    private MeshCollider collider;

    public void MeshRegenerated()
    {
        collider = GetComponent<MeshCollider>();
        cylinder = GetComponent<FZCylinder>();
        originalVertices = cylinder.Mesh.vertices.ToArray();
        modifiedVertices = cylinder.Mesh.vertices;
        normals = cylinder.Mesh.normals;
    }

    //r - (abs(d.z - c.z))

    public void AddDepression(Vector3 depressionPoint, float radius)
    {
        Vector3 worldPos = transform.worldToLocalMatrix * depressionPoint;
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            var distance = Mathf.Abs(worldPos.y - modifiedVertices[i].y);
            if(distance < radius)
            {
                Vector3 newVert;
                if(normals[i].x == 0)
                {
                    newVert = modifiedVertices[i];
                    newVert.x = modifiedVertices[i].x * (1 -  (Time.deltaTime * deformSpeed)) * (1 -  (Time.deltaTime * deformSpeed));
                    newVert.z = modifiedVertices[i].z * (1 - (Time.deltaTime * deformSpeed)) * (1 -  (Time.deltaTime* deformSpeed));
                }
                else
                {

                    /*float distToCenter = Mathf.Abs(worldPos.z - transform.position.z);
                    float delta = collider.bounds.extents.y - distToCenter;
                    newVert = modifiedVertices[i] - normals[i] * delta/50;*/
                    newVert = modifiedVertices[i] - normals[i] * Time.deltaTime * deformSpeed;
                    if (newVert.sqrMagnitude - cutFullyThreshold < (originalVertices[i] - normals[i] * maximumDepression).sqrMagnitude)
                    {
                        newVert = originalVertices[i] - normals[i] * maximumDepression;
                    }
                }
                modifiedVertices[i] = newVert;
            }
        }

        cylinder.Mesh.SetVertices(modifiedVertices);
        cylinder.collider.sharedMesh = cylinder.Mesh;
    }
}
