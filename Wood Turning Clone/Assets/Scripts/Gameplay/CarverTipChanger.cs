using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipTypes
{
    BoxTip = 0,
    TriangleTip = 1,
    ArchTip = 2
}

public class CarverTipChanger : MonoBehaviour
{
    private Carver carver;
    private void Start()
    {
        carver = FindObjectOfType<Carver>();
    }
    public void ChangeToBoxTip()
    {
        carver.ChangeTip(TipTypes.BoxTip);
    }

    public void ChangeToTriangleTip()
    {
        carver.ChangeTip(TipTypes.TriangleTip);
    }
    public void ChangeToArchTip()
    {
        carver.ChangeTip(TipTypes.ArchTip);
    }
}
