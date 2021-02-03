using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundSometimes : MonoBehaviour
{
    public AudioSource audioSource;

    public float minTime = 5f;
    public float maxTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(A());
    }

    IEnumerator A()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            audioSource.Play();
        }
    }
}
