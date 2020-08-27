using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Carve = 0,
    Paint = 1,
    Evaluate = 2
}
public class StateHandler : MonoBehaviour
{
    private State state;
    public static StateHandler instance;
    public event Action<State> OnStateChanged;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        state = State.Carve;
    }

    public void NextState()
    {
        state++;
        OnStateChanged?.Invoke(state);
    }
}
