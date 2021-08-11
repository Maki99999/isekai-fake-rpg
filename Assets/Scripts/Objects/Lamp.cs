using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour, UsesPower
{
    private Light[] lights;
    public bool isOnAtStart = false;

    [Space(10)]
    public GameObject eyes;
    public AudioSource randomOffSound;

    private bool on;
    private bool powerOn = true;
    public static float randomChance = 0.05f;

    void Awake()
    {
        on = isOnAtStart;

        Lamp[] lamps = GetComponentsInChildren<Lamp>(false);
        foreach (Lamp lamp in lamps)
            if (lamp != this)
                Destroy(lamp);

        lights = GetComponentsInChildren<Light>(false);
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
        if (on)
        {
            randomOffSound.Play();
            TurnOff();
        }
    }
}
