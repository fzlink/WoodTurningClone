using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCanvas : MonoBehaviour
{
    public Spray sprayObject;

    void Start()
    {
        sprayObject.gameObject.SetActive(false);
        StateHandler.instance.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(State state)
    {
        if (state == State.Paint)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            sprayObject.gameObject.SetActive(true);
        }
    }
}
