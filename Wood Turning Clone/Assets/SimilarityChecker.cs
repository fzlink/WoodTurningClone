using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimilarityChecker : MonoBehaviour
{
    public Text similarityText;
    public Slider similaritySlider;

    private void OnEnable()
    {
        float simRatio = FindObjectOfType<Silhouette>().GetSimRatio();
        if (simRatio >= 100f)
            simRatio = 100f;
        SetText(simRatio);
        SetSlider(simRatio);
    }

    private void SetSlider(float simRatio)
    {
        similaritySlider.value = simRatio;
    }

    private void SetText(float simRatio)
    {
        similarityText.text = "%" + Convert.ToInt32(simRatio);
    }
}
