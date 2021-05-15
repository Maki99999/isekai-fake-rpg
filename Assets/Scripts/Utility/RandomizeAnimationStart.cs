using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomizeAnimationStart : MonoBehaviour
{
    public float maxTime;
    public string animationName;

    void Start()
    {
        GetComponent<Animator>().Play(animationName, -1, Random.Range(0f, maxTime));
    }
}
