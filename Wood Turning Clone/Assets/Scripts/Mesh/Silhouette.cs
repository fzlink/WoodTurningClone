using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silhouette : MonoBehaviour
{
    public MeshFilter goalMeshFilter;
    public MeshFilter playerMeshFilter;
    public float xThreshold = 0.1f;

    private float goalMeshMinX;
    private float goalMeshMaxX;
    private Vector3[] goalVertices;
    private Vector3[] playerVertices;
    private float initialSim;
    private float simRatio;

    void Start()
    {
        StateHandler.instance.OnStateChanged += OnStateChanged;
        initialSim = CalculateSimilarityRatio(true);
    }

    private void OnStateChanged(State state)
    {
        if(state == State.Paint)
        {
            simRatio = CalculateSimilarityRatio(false);
            goalMeshFilter.GetComponent<Outline>().enabled = false;
            playerMeshFilter.GetComponent<Rotater>().OnStateChanged(state);
        }
        else if(state == State.Evaluate)
        {
            playerMeshFilter.GetComponent<Rotater>().OnStateChanged(state);
        }
    }

    public float GetSimRatio()
    {
        return simRatio;
    }


    public float CalculateSimilarityRatio(bool isInitial)
    {
        if (!isInitial && initialSim == 0) return 0f;
        playerVertices = playerMeshFilter.sharedMesh.vertices;
        goalVertices = goalMeshFilter.sharedMesh.vertices;
        ProjectToWorld(playerVertices);
        ProjectToWorld(goalVertices);
        FindGoalMinMaxX(goalVertices);
        float sim = 0f;

        for (int i = 0; i < playerVertices.Length; i++)
        {
            for (int j = 0; j < goalVertices.Length; j++)
            {
                if(Mathf.Abs(playerVertices[i].y - goalVertices[j].y) < xThreshold)
                {
                    sim += Vector3.Distance(playerVertices[i], goalVertices[j]);
                }
                else if(playerVertices[i].y < goalMeshMinX || playerVertices[i].y > goalMeshMaxX)
                {
                    sim += Vector3.Distance(playerVertices[i], Vector3.zero);
                }
            }
        }
        if (isInitial)
            return sim;
        else
        {
            if (initialSim == 0) return 0;
            return  100-((sim / initialSim) * 100);
        }
    }

    private void ProjectToWorld(Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
    }
    private void FindGoalMinMaxX(Vector3[] vertices)
    {
        goalMeshMinX = Mathf.Infinity;
        goalMeshMaxX = Mathf.NegativeInfinity;
        for (int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].y > goalMeshMaxX)
                goalMeshMaxX = vertices[i].y;
            if (vertices[i].y < goalMeshMinX)
                goalMeshMinX = vertices[i].y;
        }
        print(goalMeshMinX);
        print(goalMeshMaxX);
    }
   
}