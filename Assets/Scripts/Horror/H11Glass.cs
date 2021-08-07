using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H11Glass : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    public void FallingGlass()
    {
        animator.SetTrigger("Fall");
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
