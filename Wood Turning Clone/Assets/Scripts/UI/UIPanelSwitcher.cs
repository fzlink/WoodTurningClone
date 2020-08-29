using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelSwitcher : MonoBehaviour
{
    public GameObject colorsPanel;
    public GameObject tipsPanel;
    public GameObject nextButton;
    public GameObject similarityPanel;

    private void Start()
    {
        StateHandler.instance.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(State state)
    {
        DisablePanels();
        if(state == State.Carve)
        {
            tipsPanel.SetActive(true);
            nextButton.SetActive(true);
        }
        else if(state == State.Paint)
        {
            colorsPanel.SetActive(true);
            nextButton.SetActive(true);
        }
        else if(state == State.Evaluate)
        {
            similarityPanel.SetActive(true);
        }
    }

    private void DisablePanels()
    {
        colorsPanel.SetActive(false);
        tipsPanel.SetActive(false);
        nextButton.SetActive(false);
        similarityPanel.SetActive(false);
    }
}
