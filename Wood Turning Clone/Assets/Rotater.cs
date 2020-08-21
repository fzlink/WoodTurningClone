using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(-Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
