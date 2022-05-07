using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H7SpiderHide : MonoBehaviour, ISaveDataObject
    {
        public Animator hideAnimator;
        public Animator spiderAnimator;

        public AudioSource audioSource;

        bool active;
        bool playerNearby;

        public string saveDataId => "SpiderHide" + transform.parent.name;

        private void Update()
        {
            if (active && playerNearby)
            {
                Vector3 screenPoint = GameController.Instance.metaPlayer.cam.WorldToViewportPoint(spiderAnimator.transform.position);
                if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                {
                    Hide();
                }
            }
        }

        public void Show()
        {
            spiderAnimator.gameObject.SetActive(true);
            active = true;
        }

        public void Hide()
        {
            StartCoroutine(GameController.Instance.playerEventManager.FocusObject(false, spiderAnimator.transform, 1f));

            active = false;
            hideAnimator.SetBool("Hide", true);
            spiderAnimator.SetFloat("MovingSpeed", 10f);

            audioSource.PlayDelayed(0.5f);
        }

        public void Hide2()
        {
            active = false;
            if (hideAnimator.isActiveAndEnabled)
                hideAnimator.SetBool("Hide", false);
            spiderAnimator.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                playerNearby = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerNearby = false;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("active", active);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
            {
                Hide2();
                return;
            }
            if (dictEntry.GetBool("active", false))
                Show();
            else
                Hide2();
        }
    }
}
