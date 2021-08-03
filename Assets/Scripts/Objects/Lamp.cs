using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour, UsesPower
{
    public Light[] lights;
    public bool isOnAtStart = false;
    private bool on;
    private bool powerOn = true;

    void Awake()
    {
        on = isOnAtStart;
        foreach (Light light in lights)
            light.enabled = on;
    }

    public void SetPower(bool powerOn)
    {
        this.powerOn = powerOn;
        if (!powerOn)
            foreach (Light light in lights)
                light.enabled = false;
        else
            foreach (Light light in lights)
                light.enabled = on;
    }

    public void TurnOn()
    {
        on = true;

        if (powerOn)
            foreach (Light light in lights)
                light.enabled = on;
    }

    public void TurnOff()
    {
        on = false;

        if (powerOn)
            foreach (Light light in lights)
                light.enabled = on;
    }

    public void Toggle()
    {
        if (on)
            TurnOff();
        else
            TurnOn();
    }
}
