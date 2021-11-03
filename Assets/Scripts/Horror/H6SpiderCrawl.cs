using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H6SpiderCrawl : MonoBehaviour
    {
        public Animator crawlAnimator;
        public Animator spiderAnimator;

        public AudioSource audioSource;

        public H7SpiderHide[] smallSpiders;

        public void Crawl()
        {
            crawlAnimator.SetTrigger("Crawl");
            spiderAnimator.SetFloat("MovingSpeed", 1f);

            audioSource.PlayDelayed(3f);

            foreach (H7SpiderHide smallSpider in smallSpiders)
                smallSpider.Show();
        }
    }
}
