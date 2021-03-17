using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioFx : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;
    public Vector2 timeRange;

    void Start()
    {
        StartCoroutine(AudioFx());
    }

    IEnumerator AudioFx()
    {
        yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
        while (enabled)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
            yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
        }
    }
}
