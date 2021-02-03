using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBLight : MonoBehaviour
{
    public Light[] lights;

    public float step = 0.001f;

    float r = 0f;
    float g = 1f;
    float b = 1f;

    bool reduceR = true;
    bool reduceG = false;
    bool reduceB = false;

    void Update()
    {
        foreach (Light light in lights)
        {
            light.color = new Color(r, g, b);
        }
        if (reduceR)
        {
            if (r <= 0f)
            {
                r = 0f;
                reduceR = false;
                reduceG = true;
                Update();
            }
            else
            {
                r -= step;
                b += step;
            }
        }
        else if (reduceG)
        {
            if (g <= 0f)
            {
                g = 0f;
                reduceG = false;
                reduceB = true;
                Update();
            }
            else
            {
                g -= step;
                r += step;
            }
        }
        else if (reduceB)
        {
            if (b <= 0f)
            {
                b = 0f;
                reduceB = false;
                reduceR = true;
                Update();
            }
            else
            {
                b -= step;
                g += step;
            }
        }
    }
}
