using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    Light lightComp;
    public Gradient randColors;

    void Start()
    {
        lightComp = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (enabled)
        {
            lightComp.color = randColors.Evaluate(Random.value);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
