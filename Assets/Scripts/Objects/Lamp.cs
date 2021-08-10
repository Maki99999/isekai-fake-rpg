using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour, UsesPower
{
    public Light[] lights;
    public bool isOnAtStart = false;

    [Space(10)]
    public GameObject eyes;

    private bool on;
    private bool powerOn = true;
    private const float randomChance = 0.05f;

    void Awake()
    {
        on = isOnAtStart;
        foreach (Light light in lights)
            light.enabled = on;
    }

    void Start()
    {
        if (eyes != null)
            eyes.SetActive(false);
    }

    public void SetPower(bool powerOn)
    {
        this.powerOn = powerOn;
        if (powerOn)
            foreach (Light light in lights)
                light.enabled = on;
        else
        {
            foreach (Light light in lights)
                light.enabled = false;
            if (eyes != null)
                eyes.SetActive(false);
        }
    }

    public void TurnOn()
    {
        on = true;

        if (powerOn)
            foreach (Light light in lights)
                light.enabled = on;

        if (eyes != null)
            eyes.SetActive(false);

        if (Random.value < randomChance)
            StartCoroutine(TurnOffAfter());
    }

    public void TurnOff()
    {
        on = false;

        if (powerOn)
            foreach (Light light in lights)
                light.enabled = on;

        if (eyes != null && Random.value < randomChance)
            eyes.SetActive(true);
    }

    public void Toggle()
    {
        StopAllCoroutines();
        if (on)
            TurnOff();
        else
            TurnOn();
    }

    IEnumerator TurnOffAfter()
    {
        yield return new WaitForSeconds(Random.Range(7f, 17f));
        TurnOff();
    }
}
