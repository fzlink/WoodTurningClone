using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float carveRotationSpeed;
    public float paintRotationSpeed;
    private float currentRotationSpeed;
    private bool canRotate = true;

    private void Start()
    {
        currentRotationSpeed = carveRotationSpeed;
    }

    void Update()
    {
        if (!canRotate) return;
        transform.Rotate(-Vector3.up, Time.deltaTime * currentRotationSpeed);
    }

    public void OnStateChanged(State state)
    {
        if (state == State.Carve)
        {
            currentRotationSpeed = carveRotationSpeed;
        }
        else if(state == State.Paint)
        {
            GetToPaintPosition();
            currentRotationSpeed = paintRotationSpeed;
        }
        else if(state == State.Evaluate)
        {
            canRotate = false;
        }
    }

    private void GetToPaintPosition()
    {
        transform.position = new Vector3(-1f, 0f, 0f);
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }
}
