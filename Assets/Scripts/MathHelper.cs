using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper 
{
    public static float CalculateDistance(float x1, float y1, float x2, float y2)
    {
        // Calculate the squared differences
        float dx = x2 - x1;
        float dy = y2 - y1;
        float squaredDistance = dx * dx + dy * dy;

        // Return the square root of the squared distance
        return Mathf.Sqrt(squaredDistance);
    }
}
