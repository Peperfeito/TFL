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
        if (alignMode == AlignMode.Center)
        {
            positionBuffer.y = (Mathf.Floor(Mathf.Abs(positionBuffer.y)) + .5f) * (positionBuffer.y / Mathf.Abs(positionBuffer.y));
        }
        else
        {
            positionBuffer.y = Mathf.Round(positionBuffer.y) + .3f;
        }
        target.position = positionBuffer;
    }
}
