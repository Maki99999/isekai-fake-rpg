using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H6SpiderCrawl : MonoBehaviour, ISaveDataObject
    {
        public Animator crawlAnimator;
        public Animator spiderAnimator;

        public AudioSource audioSource;

        public H7SpiderHide[] smallSpiders;
        public GameObject tinySpiderBatches;

        private bool triggered = false;

        public string saveDataId => "SpiderCrawl";

        public void Crawl()
        {
            crawlAnimator.SetTrigger("Crawl");
            spiderAnimator.SetFloat("MovingSpeed", 1f);
            StartCoroutine(GameController.Instance.playerEventManager.FocusObject(false, spiderAnimator.transform, 6f));

            audioSource.PlayDelayed(3f);

            foreach (H7SpiderHide smallSpider in smallSpiders)
                smallSpider.Show();
            tinySpiderBatches.SetActive(true);

            triggered = true;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("triggered", triggered);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            if (dictEntry.GetBool("triggered", false))
                tinySpiderBatches.SetActive(true);
        }
    }
}
