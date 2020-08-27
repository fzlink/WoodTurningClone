using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerPanel : MonoBehaviour
{
    public List<Color> colors;
    public List<Button> buttons;
    private static Color currentColor;

    void Start()
    {
        if(colors.Count > transform.childCount)
        {
            colors = colors.GetRange(0, transform.childCount);
        }
        SetButtonColors();
        SetButtonListeners();
        SetCurrentColor(0);
    }

    private void SetButtonListeners()
    {
        int i = 0;
        foreach (Button button in buttons)
        {
            int k = i++;
            button.onClick.AddListener(() => SetCurrentColor(k));
        }
    }

    private void SetButtonColors()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            if(transform.GetChild(i) != null)
                transform.GetChild(i).GetComponent<Image>().color = colors[i];
        }
    }

    public void SetCurrentColor(int buttonInd)
    {
        currentColor = colors[buttonInd];
    }

    public static Color GetCurrentColor()
    {
        return currentColor;
    }
}
