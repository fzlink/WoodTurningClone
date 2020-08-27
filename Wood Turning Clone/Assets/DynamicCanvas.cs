using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCanvas : MonoBehaviour
{


    void Start()
    {
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
        }
    }
}
