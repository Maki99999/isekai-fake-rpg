using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troete : MonoBehaviour
{
    public AudioSource audioSource;
    public float range = 10f;

    Vector3 origPos;
    
    private void Start() {
        origPos = transform.position;
        StartCoroutine(A());
    }

    IEnumerator A() {
        while(enabled) {
            transform.position = origPos + Random.insideUnitSphere * range;
            audioSource.Play();
            yield return new WaitForSeconds(Random.Range(5f, 20f));
        }
    }
}
