using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlignMode
{
    Center,
    Bottom,
}

public static class FarueiUtils
{
    public static void AlignWithGrid(Transform target, AlignMode alignMode)
    {
        Vector3 positionBuffer = target.position;
        positionBuffer.x = (Mathf.Floor(Mathf.Abs(positionBuffer.x)) + .5f) * (positionBuffer.x / Mathf.Abs(positionBuffer.x));
        positionBuffer.y = Mathf.Round(positionBuffer.y) + (alignMode == AlignMode.Center ? .5f : .3f);
        target.position = positionBuffer;
    }
}
