using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H6SpiderCrawl : MonoBehaviour
{
    public Animator crawlAnimator;
    public Animator spiderAnimator;

    public AudioSource audioSource;

    public GameObject[] smallSpiders;

    public void Crawl()
    {
        crawlAnimator.SetTrigger("Crawl");
        spiderAnimator.SetFloat("MovingSpeed", 1f);

        audioSource.PlayDelayed(3f);

        foreach (GameObject smallSpider in smallSpiders)
        {
            smallSpider.SetActive(true);
        }
    }
}
